using AssetManager.Data;
using AssetManager.Perforce;
using AssetManager.View;
using AssetManager.Wpf;
using CommunityToolkit.Mvvm.Input;
using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Animations;
using HelixToolkit.Wpf.SharpDX.Assimp;
using HelixToolkit.Wpf.SharpDX.Controls;
using HelixToolkit.Wpf.SharpDX.Model;
using HelixToolkit.Wpf.SharpDX.Model.Scene;
using SharpDX;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Threading;

using Camera = HelixToolkit.Wpf.SharpDX.Camera;
using OrthographicCamera = HelixToolkit.Wpf.SharpDX.OrthographicCamera;
using PerspectiveCamera = HelixToolkit.Wpf.SharpDX.PerspectiveCamera;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows;
using System;

namespace AssetManager.ViewModel
{
    class AssetManagerViewModel : ViewModelBase, IDisposable
    {
        private string OpenFileFilter = $"{HelixToolkit.Wpf.SharpDX.Assimp.Importer.SupportedFormatsString}";
        private string ExportFileFilter = $"{HelixToolkit.Wpf.SharpDX.Assimp.Exporter.SupportedFormatsString}";

        private ObservableCollection<ObjectDisplayWrapper> _objectDisplay;
        public ObservableCollection<ObjectDisplayWrapper> ObjectDisplay
        { 
            get => _objectDisplay;
            set => SetProperty(ref _objectDisplay, value);
        }

        private ObservableCollection<string> _workspaces;
        public ObservableCollection<string> Workspaces
        {
            get => _workspaces;
            set => SetProperty(ref _workspaces, value);
        }

        private bool _isConnected;
        public bool IsConnected
        {
            get => _isConnected;
            set => SetProperty(ref _isConnected, value);
        }

        #region HelixComponents
        public const string Orthographic = "Orthographic Camera";
        public const string Perspective = "Perspective Camera";

        private string _cameraModel;
        public string CameraModel
        {
            get => _cameraModel;
            set
            {
                SetProperty(ref _cameraModel, value);

            }
        }
        private Camera _camera;
        public Camera Camera
        {
            get => _camera;
            set
            {
                SetProperty(ref _camera, value);
                CameraModel = _camera is PerspectiveCamera
                                ? Perspective 
                                : _camera is OrthographicCamera ? Orthographic : null;
            }
        }

        private IEffectsManager _effectsManager;
        public IEffectsManager EffectsManager
        {
            get => _effectsManager;
            set => SetProperty(ref _effectsManager, value);
        }

        private Point3D _modelCentroid = default;
        public Point3D ModelCentroid
        {
            get => _modelCentroid;
            private set => SetProperty(ref _modelCentroid, value);
        }

        private BoundingBox _modelBound = new BoundingBox();
        public BoundingBox ModelBound
        {
            get => _modelBound;
            private set => SetProperty(ref _modelBound, value);
        }

        private HelixToolkitScene scene;
        private SynchronizationContext context = SynchronizationContext.Current;
        public SceneNodeGroupModel3D GroupModel { get; } = new SceneNodeGroupModel3D();

        public TextureModel EnvironmentMap { get; private set; }

        private bool _renderEnvironmentMap = true;
        public bool RenderEnvironmentMap
        {
            get => _renderEnvironmentMap;
            set
            {
                SetProperty(ref _renderEnvironmentMap, value);
                if (_renderEnvironmentMap && scene != null && scene.Root != null)
                {
                    foreach (var node in scene.Root.Traverse())
                    {
                        if (node is MaterialGeometryNode m && m.Material is PBRMaterialCore material)
                        {
                            material.RenderEnvironmentMap = value;
                        }
                    }
                }
            }
        }
        #endregion

        #region Commands
        public ICommand SyncCommand => new RelayCommand(Sync);
        public ICommand OpenRootFolderCommand => new RelayCommand(OpenRootFolder);
        public ICommand PerforceSetupCommand => new RelayCommand(PerforceLoginAndSetup);
        public ICommand LoadFileCommand => new RelayCommand(LoadFile);
        #endregion

        public AssetManagerViewModel()
        {
            EffectsManager = new DefaultEffectsManager();
            Camera = new OrthographicCamera()
            {
                LookDirection = new Vector3D(0, -10, -10),
                Position = new Point3D(0, 10, 10),
                UpDirection = new Vector3D(0, 1, 0),
                FarPlaneDistance = 5000,
                NearPlaneDistance = 0.1f
            };
            EnvironmentMap = TextureModel.Create("Resources/Cubemap_Grandcanyon.dds");
        }

        ~AssetManagerViewModel()
        {
            Dispose();
        }

        private void Sync()
        {
            //PerforceTools.Sync();
        }

        private void OpenRootFolder()
        {
            
        }

        private void SetupPerforceDependencies()
        {
            Workspaces = Utils.Utils.ToObservableCollection(PerforceTools.GetUserWorkspaces());
        }

        private void PerforceLoginAndSetup()
        {
            PerforceLoginSetup p4Win = new PerforceLoginSetup();
            PerforceLoginViewModel vm = new PerforceLoginViewModel();
            p4Win.DataContext = vm;
            p4Win.ShowDialog();

            // TODO: this is temporary. Find a good way to do this update...
            IsConnected = PerforceTools.Connection.connectionEstablished();
            if (IsConnected)
                SetupPerforceDependencies();
        }

        private string OpenFileDialog(string filter)
        {
            var d = new OpenFileDialog();
            d.CustomPlaces.Clear();

            d.Filter = filter;

            if (!d.ShowDialog().Value)
            {
                return null;
            }

            return d.FileName;
        }

        private void LoadFile()
        {
            //if (isLoading)
            //{
            //    return;
            //}
            string path = OpenFileDialog(OpenFileFilter);
            if (path == null)
            {
                return;
            }
            //StopAnimation();
            var syncContext = SynchronizationContext.Current;
            //IsLoading = true;
            Task.Run(() =>
            {
                Importer loader = new Importer();
                HelixToolkitScene scene = loader.Load(path);
                //scene.Root.Attach(EffectsManager); // Pre attach scene graph
                //scene.Root.UpdateAllTransformMatrix();
                //if (scene.Root.TryGetBound(out var bound))
                //{
                //    /// Must use UI thread to set value back.
                //    syncContext.Post((o) => { ModelBound = bound; }, null);
                //}
                //if (scene.Root.TryGetCentroid(out var centroid))
                //{
                //    /// Must use UI thread to set value back.
                //    syncContext.Post((o) => { ModelCentroid = centroid.ToPoint3D(); }, null);
                //}
                return scene;
            }).ContinueWith((result) =>
            {
                //IsLoading = false;
                if (result.IsCompleted)
                {
                    scene = result.Result;
                    //Animations.Clear();
                    SceneNode[] oldNode = GroupModel.SceneNode.Items.ToArray();
                    GroupModel.Clear();
                    Task.Run(() =>
                    {
                        foreach (var node in oldNode)
                        { node.Dispose(); }
                    });
                    if (scene != null)
                    {
                        if (scene.Root != null)
                        {
                            foreach (var node in scene.Root.Traverse())
                            {
                                if (node is MaterialGeometryNode m)
                                {
                                    //m.Geometry.SetAsTransient();
                                    if (m.Material is PBRMaterialCore pbr)
                                    {
                                        pbr.RenderEnvironmentMap = RenderEnvironmentMap;
                                    }
                                    else if (m.Material is PhongMaterialCore phong)
                                    {
                                        phong.RenderEnvironmentMap = RenderEnvironmentMap;
                                    }
                                }
                            }
                        }
                        GroupModel.AddNode(scene.Root);
                        //if (scene.HasAnimation)
                        //{
                        //    var dict = scene.Animations.CreateAnimationUpdaters();
                        //    foreach (var ani in dict.Values)
                        //    {
                        //        Animations.Add(ani);
                        //    }
                        //}
                        //foreach (var n in scene.Root.Traverse())
                        //{
                        //    n.Tag = new AttachedNodeViewModel(n);
                        //}
                        //FocusCameraToScene();
                    }
                }
                else if (result.IsFaulted && result.Exception != null)
                {
                    MessageBox.Show(result.Exception.Message);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #region Dispose
        private bool _disposingValue = false;
        public void Dispose()
        {
            if (!_disposingValue)
            {
                if (EffectsManager != null)
                {
                    var effectManager = EffectsManager as IDisposable;
                    Disposer.RemoveAndDispose(ref effectManager);
                }
                _disposingValue = true;
                GC.SuppressFinalize(this);
            }
        }
        #endregion
    }
}
