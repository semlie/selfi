using Camera_NET;
using ConfiguratinService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace CameraService
{
    public class CameraMeneger
    {
        private CameraChoice _CameraChoice = new CameraChoice();
        private Camera _Camera = new Camera();
        public CameraMeneger()
        {
            InitCameraSeting();
        }

        private void InitCameraSeting()
        {
            _CameraChoice.UpdateDeviceList();
            if (_CameraChoice == null)
            {
                throw new Exception("No camara in this system");
            }


        }
        public Resolution GetResolution(IMoniker moniker)
        {
            ChackIfCameraExist();
            var resolution = ConfigurationSettingManager.GetConfigurtionSetting("Resolution");
            ResolutionList resolutionList = Camera.GetResolutionList(moniker);
            return resolutionList[int.Parse(resolution)];
        }

        private void ChackIfCameraExist()
        {
            if (_CameraChoice == null)
            {
                InitCameraSeting();
            }
        }


        public IMoniker GetDevice()
        {
            ChackIfCameraExist();
            var devicesId = ConfigurationSettingManager.GetConfigurtionSetting("VideoDeviceId");

                var device = _CameraChoice.Devices[int.Parse(devicesId)].Mon;

            return device;
        }
    }
}
