using System;
using System.Collections.Generic;
using System.Linq;
using Perforce.P4;

namespace AssetManager.Perforce
{
    public static class PerforceTools
    {
        public static Connection Connection;
        public static Repository Repository;

        public static bool Connect(string server, string user, string password = "")
        {
            Options options = new Options();

            Server p4Server = new Server(new ServerAddress(server));

            Repository = new Repository(p4Server);
            Connection = Repository.Connection;
            Connection.UserName = user;

            bool connected = Connection.Connect(options);

            if (!string.IsNullOrEmpty(password))
            {
                Credential cred = Connection.Login(password, null, null);
                if (cred != null)
                    Connection.Credential = cred;
            }
   
            return connected;
        }

        public static void Sync(string depotPath, out IList<FileSpec> syncedFiles)
        {
            FileSpec filesToSync = new FileSpec(new DepotPath(depotPath), null, null, null);
            SyncFilesCmdOptions syncOptions = new SyncFilesCmdOptions(SyncFilesCmdFlags.None);
            syncedFiles = Connection.Client.SyncFiles(syncOptions, filesToSync);
        }

        public static void EditFiles(IList<string> files, out IList<FileSpec> checkedOutFiles)
        {
            List<FileSpec> filesToEdit = new List<FileSpec>();
            foreach (string file in files)
            {
                filesToEdit.Add(new FileSpec(null, null, new LocalPath(file), null));
            }

            int defaultChangelist = 0;
            EditCmdOptions editOptions = new EditCmdOptions(EditFilesCmdFlags.None, defaultChangelist, null);
            checkedOutFiles = Connection.Client.EditFiles(editOptions, filesToEdit.ToArray());
        }

        public static void MarkForAddFiles(IList<string> files, out IList<FileSpec> addedFiles)
        {
            List<FileSpec> filesToAdd = new List<FileSpec>();
            foreach (string file in files)
            {
                filesToAdd.Add(new FileSpec(null, null, new LocalPath(file), null));
            }

            int defaultChangelist = 0;
            AddFilesCmdOptions markForAddOptions = new AddFilesCmdOptions(AddFilesCmdFlags.None, defaultChangelist, null);
            addedFiles = Connection.Client.AddFiles(markForAddOptions, filesToAdd.ToArray());
        }

        public static IList<string> GetUserWorkspaces(string user = "", string server = "")
        {
            List<string> workspaces = new List<string>();
            if (string.IsNullOrEmpty(user))
            {
                user = Connection.UserName;
            }

            if (string.IsNullOrEmpty (server))
            {
                server = Connection.Server.Address.Uri;
            }
            ClientsCmdOptions opts = new ClientsCmdOptions(ClientsCmdFlags.None, user, null, 0, null);
            
            IList<Client> clients = null;
            try
            {
                clients = Repository.GetClients(opts);
                foreach (Client client in clients)
                {
                    workspaces.Add(client.Name);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return workspaces;
        }

        public static void SetClient(string client)
        {
            if (Repository.GetClients(null).Any(f => f.Name == client))
            {
                Connection.SetClient(client);
            }
            else
            {
                throw new P4Exception(ErrorSeverity.E_FAILED, "Client isn't in the repository");
            }
        }
    }
}
