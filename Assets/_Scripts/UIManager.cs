using System.Collections;
using UnityEngine;

namespace NDRCellularAutomata
{
    public class UIManager : MonoBehaviour
    {
        #region Singleton
        public static UIManager instance;
        private void Awake()
        {
            if (!instance)
                instance = this;
            else
                Destroy(this);
        }
        #endregion

        [SerializeField] SaveDialog saveDialog;
        [SerializeField] LoadDialog loadDialog;

        bool isActive;

        public SaveDialog SaveDialog { get => saveDialog; }
        public bool IsActive { get => isActive; set => isActive = value; }
        public LoadDialog LoadDialog { get => loadDialog; set => loadDialog = value; }

        private void Start()
        {
            saveDialog.gameObject.SetActive(false);
            loadDialog.gameObject.SetActive(false);
        }

        public void ShowSaveDialog()
        {
            saveDialog.gameObject.SetActive(true);
            isActive = true;
        }
        public void ShowLoadDialog()
        {
            loadDialog.gameObject.SetActive(true);
            isActive = true;
        }

    }
}