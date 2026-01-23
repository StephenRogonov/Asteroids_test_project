using System;

namespace _Project.Scripts.Bootstrap.LoadOptions
{
    public class LoadOptionsPresenter : IDisposable
    {
        private LoadOptionsModel _model;
        private LoadOptionsView _view;

        public LoadOptionsPresenter(LoadOptionsModel loadOptionsModel)
        {
            _model = loadOptionsModel;
        }

        public void Init(LoadOptionsView loadOptionsView)
        {
            _view = loadOptionsView;

            _view.LocalClicked += LoadLocalSave;
            _view.CloudClicked += LoadCloudSave;
            _model.ShowView += EnableView;
        }

        private void EnableView(string local, string cloud)
        {
            _view.DisplayLocalSaveDate(local);
            _view.DisplayCloudSaveDate(cloud);
            _view.EnableObject();
        }

        private void LoadLocalSave()
        {
            _model.UseSelectedLoadOption(false);
        }

        private void LoadCloudSave()
        {
            _model.UseSelectedLoadOption(true);
        }

        public void Dispose()
        {
            _view.LocalClicked -= LoadLocalSave;
            _view.CloudClicked -= LoadCloudSave;
            _model.ShowView -= EnableView;
        }
    }
}
