using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component for creating and filling up numerical matrices
/// </summary>
public class MatrixBuilder : MonoBehaviour
{
    public List<Matrix4x4> matrixList = new List<Matrix4x4>();
    public List<MatrixVisualInstance> matrixPrimitives = new List<MatrixVisualInstance>();

    public Color matrixVisualizationColor = Color.white;
    public TextAsset jsonFile;

    /// <summary>
    /// Use a JSON-asset with values to create matrices. File must be based on <see cref="MatrixFormat"/>.
    /// </summary>
    /// <param name="textAsset"></param>
    public void BuildMatricesFromAssetFile(TextAsset textAsset)
    {
        //Get raw data objects from JSON file
        MatrixFormat[] readMatrices = Newtonsoft.Json.JsonConvert.DeserializeObject<MatrixFormat[]>(textAsset.text);

        //Read raw data
        for (int i = 0; i < readMatrices.Length; i++)
        {
            //Store matrix being read to reduce calls to array
            MatrixFormat currentMatrix = readMatrices[i];

            Matrix4x4 matrix = new Matrix4x4()
            {
                m00 = currentMatrix.m00, m01 = currentMatrix.m01, m02 = currentMatrix.m02, m03 = currentMatrix.m03,
                m10 = currentMatrix.m10, m11 = currentMatrix.m11, m12 = currentMatrix.m12, m13 = currentMatrix.m13,
                m20 = currentMatrix.m20, m21 = currentMatrix.m21, m22 = currentMatrix.m22, m23 = currentMatrix.m23,
                m30 = currentMatrix.m30, m31 = currentMatrix.m31, m32 = currentMatrix.m32, m33 = currentMatrix.m33,
            };

            //Create Matrix based on this numerical sequence and store it;
            matrixList.Add(matrix);

            //Set up visual instances to display matrix values
            matrixPrimitives.Add(MatrixVisualInstance.CreateNewInstance(matrix, matrixVisualizationColor));
        }
    }
}
