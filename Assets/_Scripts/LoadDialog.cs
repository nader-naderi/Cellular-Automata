using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

namespace NDRCellularAutomata
{
    public class LoadDialog : MonoBehaviour
    {
        [SerializeField] Dropdown patternName;

        public Dropdown PatternName { get => patternName; }

        private void Start()
        {
            ReloadOptions();
        }

        private void OnEnable()
        {
            ReloadOptions();
        }

        void ReloadOptions()
        {
            List<string> options = new List<string>();

            string[] filePaths = Directory.GetFiles(@"pattern/");

            for (int i = 0; i < filePaths.Length; i++)
            {
                string fileName = filePaths[i].Substring(filePaths[i].LastIndexOf('/') + 1);
                string extension = System.IO.Path.GetExtension(fileName);

                fileName = fileName.Substring(0, fileName.Length - extension.Length);

                options.Add(fileName);
            }

            patternName.ClearOptions();
            patternName.AddOptions(options);
        }

        public void OnBtnClick_CloseWindow()
        {
            UIManager.instance.IsActive = false;
            gameObject.SetActive(false);
        }

        public void OnBtnClick_LoadPattern()
        {
            EventManager.TriggerEvent("LoadPattern");

            UIManager.instance.IsActive = false;
            gameObject.SetActive(false);
        }
    }
}