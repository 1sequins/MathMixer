using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class EquationGenerator : MonoBehaviour
{
    public Text equationText;

    void Start()
    {
        equationText.text = "";
    }

    public void UpdateEquation(LinkedPath path)
    {
        equationText.text = GetEquationString(path);
    }

    public string GetEquationString(LinkedPath path, bool forceValid = false)
    {
        StringBuilder sb = new StringBuilder();

        if (path != null)
        {
            GameTile[] pathArr = path.TileStack.ToArray();

            for (int i = pathArr.Length - 1; i >= 0; i--)
            {
                GameTile tile = pathArr[i];
                sb.Append(tile.Value);

                if (i >= 1)
                {
                    sb.Append(" ");
                }
            }
        }

        return sb.ToString();
    }
}
