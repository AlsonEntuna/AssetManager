using System.Collections.Generic;
using Perforce.P4;

namespace AssetManager.Perforce
{
    public static class PerforceTools
    {
        public static Connection Connection;
        public static Repository Repository;

        public static bool Login(string user, string pass)
        {
            return false;
        }
        public static bool Connect(string server, string user)
        {
            Options options = new Options();

            Server p4Server = new Server(new ServerAddress(server));

            Repository = new Repository(p4Server);
            Connection = Repository.Connection;
            Connection.UserName = user;


            return Connection.Connect(options);
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
            Options opts = new Options();
            opts["-s"] = server;
            opts["-u"] = user;
            var clients = Repository.GetClients(opts);
            return workspaces;
        }

        public static void SubmitChangelist(int changelistNo, out SubmitResults results)
        {
            
            results = null;
        }
    }
}
