using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace NDRCellularAutomata
{
    public class SaveDialog : MonoBehaviour
    {
        [SerializeField] InputField patternInout;

        public InputField PatternInout { get => patternInout; }

        public void OnBtnClick_SavePattern()
        {
            EventManager.TriggerEvent("SavePattern");

            UIManager.instance.IsActive = false;
            gameObject.SetActive(false);
        }

        public void OnBtnClick_CloseWindow()
        {
            UIManager.instance.IsActive = false;
            gameObject.SetActive(false);
        }

    }
}