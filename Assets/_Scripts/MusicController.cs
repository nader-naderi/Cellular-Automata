using System.Collections;
using UnityEngine;

namespace NDRCellularAutomata
{
    public class MusicController : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }


    }
}