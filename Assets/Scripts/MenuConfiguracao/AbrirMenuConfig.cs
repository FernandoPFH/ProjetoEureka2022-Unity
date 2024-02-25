using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class AbrirMenuConfig : MonoBehaviour
{
    [HideInInspector] public bool habilitavel = true;
    [SerializeField] private GameObject menuConfig;

    public InputDevice _rightController;

    // Start is called before the first frame update
    void Start()
    {
        menuConfig.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!habilitavel)
            return;

        if (!_rightController.isValid) {
            List<InputDevice> devices = new List<InputDevice>();

            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right,devices);

            if (devices.Count > 0) {
                _rightController = devices[0];
            }
        }

        if (_rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButton) && _rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButton) && _rightController.TryGetFeatureValue(CommonUsages.gripButton, out bool gripButton) && _rightController.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerButton)) {
            if (primaryButton && secondaryButton && gripButton && triggerButton)
                menuConfig.SetActive(true);
        }
    }

    public void Desabilitavel() {
        habilitavel = false;

        menuConfig.SetActive(false);
    }
}
