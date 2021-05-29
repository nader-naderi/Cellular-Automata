using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NDRCellularAutomata
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] CanvasGroup mainPanel;
        [SerializeField] CanvasGroup gameConfigurationPanel;

        [SerializeField] Text gameTypeTxt;

        [SerializeField] string[] cellualrAutomataons = new string[] { "Conway's Game of Life", "Brian's Brain", "Langton's ant" };

        int currentLvlIndex = 0;

        [SerializeField] Text statusTxt;

        [SerializeField] AudioSource uiSource;
        [SerializeField] AudioClip selectClip;
        [SerializeField] AudioClip confirmClip;
        [SerializeField] AudioClip closeClip;
        [SerializeField] AudioClip cancelClip;
        [SerializeField] AudioClip playClip;


        public void OnBtnClick_CreateGame()
        {
            gameConfigurationPanel.gameObject.SetActive(true);
            //mainPanel.interactable = false;
            mainPanel.GetComponent<Animator>().Play("FadeOut");
            gameConfigurationPanel.GetComponent<Animator>().Play("FadeIn");
            gameTypeTxt.text = cellualrAutomataons[currentLvlIndex];
            StatusConfig();
            uiSource.PlayOneShot(confirmClip);
        }

        public void OnBtnClick_BackToMainPanel()
        {
            //mainPanel.interactable = true;
            mainPanel.GetComponent<Animator>().Play("FadeIn");
            gameConfigurationPanel.GetComponent<Animator>().Play("FadeOut");
            //gameConfigurationPanel.gameObject.SetActive(false);
            uiSource.PlayOneShot(closeClip);
        }

        public void OnBtnClick_NextLvl()
        {
            currentLvlIndex++;

            if (currentLvlIndex > cellualrAutomataons.Length - 1)
                currentLvlIndex = 0;

            gameTypeTxt.text = cellualrAutomataons[currentLvlIndex];
            StatusConfig();
            uiSource.PlayOneShot(selectClip);
        }

       

        public void OnBtnClick_PreviousLvl()
        {
            currentLvlIndex--;
            if (currentLvlIndex < 0)
                currentLvlIndex = cellualrAutomataons.Length - 1;

            gameTypeTxt.text = cellualrAutomataons[currentLvlIndex];
            StatusConfig();
            uiSource.PlayOneShot(selectClip);
        }
        private void StatusConfig()
        {
            if (gameTypeTxt.text == "Conway's Game of Life")
            {
                statusTxt.text = "Ready to Play.";
                statusTxt.color = Color.green;
            }
            else
            {
                statusTxt.text = "Under Heavy Development ...";
                statusTxt.color = Color.red;
            }
        }

        public void OnBtnCick_PlayGame()
        {
            uiSource.PlayOneShot(playClip); 
            if(gameTypeTxt.text == "Conway's Game of Life")
            SceneManager.LoadScene("Conway's Game of Lige");
        }
    }
}