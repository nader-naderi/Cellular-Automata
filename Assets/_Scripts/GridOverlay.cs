using UnityEngine;

namespace NDRCellularAutomata
{
    public class GridOverlay : MonoBehaviour
    {
        private Material lineMaterial;

        [SerializeField] bool showMain = true;
        [SerializeField] bool showSub = false;

        [SerializeField] int gridSizeX;
        [SerializeField] int gridSizeY;

        [SerializeField] float startX;
        [SerializeField] float startY;
        [SerializeField] float startZ;

        [SerializeField] float smallStep;
        [SerializeField] float largeStep;

        [SerializeField] Color mainColor = new Color(0f, 1f, 0f, 1f);
        [SerializeField] Color subColor = new Color(0f, 0.5f, 0f, 1f);



        void CreateLineMaterial()
        {
            if(!lineMaterial)
            {
                var shader = Shader.Find("Hidden/Internal-Colored");
                lineMaterial = new Material(shader);

                lineMaterial.hideFlags = HideFlags.HideAndDontSave;

                lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

                lineMaterial.SetInt("_ZWrite", 0);

                lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);

            }
        }

        private void OnDisable()
        {
            DestroyImmediate(lineMaterial);
        }

        private void OnPostRender()
        {
            CreateLineMaterial();

            lineMaterial.SetPass(0);

            GL.Begin(GL.LINES);

            if (showSub)
            {
                GL.Color(subColor);

                for (float y = 0; y <= gridSizeY; y += smallStep)
                {
                    GL.Vertex3(startX, startY + y, startZ);
                    GL.Vertex3(startX + gridSizeX, startY + y, startZ);
                }

                for (float x = 0; x < gridSizeX; x += smallStep)
                {
                    GL.Vertex3(startX + x, startY, startZ);
                    GL.Vertex3(startX + x, startY + gridSizeY, startZ);
                }
            }

            if (showMain)
            {
                GL.Color(mainColor);

                for (float y = 0; y <= gridSizeY; y += largeStep)
                {
                    GL.Vertex3(startX, startY + y, startZ);
                    GL.Vertex3(startX + gridSizeX, startY + y, startZ);
                }

                for (float x = 0; x < gridSizeX; x += largeStep)
                {
                    GL.Vertex3(startX + x, startY, startZ);
                    GL.Vertex3(startX + x, startY + gridSizeY, startZ);
                }
            }

            GL.End();
        }
    }
}