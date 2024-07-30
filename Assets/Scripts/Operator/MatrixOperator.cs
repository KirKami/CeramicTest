using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Performs operations on matrices
/// </summary>
public class MatrixOperator : MonoBehaviour
{
    //Offset values that were found during calculations
    public List<Vector3> offsets = new List<Vector3>();

    //Builders references
    public MatrixBuilder modelBuilder;
    public MatrixBuilder spaceBuilder;

    //UI reference
    public TMP_InputField inputField;

    /// <summary>
    /// Find possible offset vectors of matrix among space matrices
    /// </summary>
    /// <param name="matrixIndex">Index of model matrix that is being searched</param>
    public void FindOffsetVectors(int matrixIndex)
    {
        offsets = new List<Vector3>();

        Matrix4x4 modelMatrix = modelBuilder.matrixList[matrixIndex];

        foreach (var spaceMatrix in spaceBuilder.matrixList)
        {
            var transformMatrix = modelMatrix.inverse* spaceMatrix;
            offsets.Add((Vector3)transformMatrix.GetColumn(3));
        }

        modelBuilder.matrixPrimitives[matrixIndex].StartTranslationSequence(modelMatrix, offsets);
    }
    /// <summary>
    /// Find possible offset vectors of matrix among space matrices. Uses input data from UI.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Input value is out of index range</exception>
    /// <exception cref="ArgumentException">Not a number</exception>
    public void FindOffsetVectors()
    {
        bool validInput = Int32.TryParse(inputField.text, out int inputIndex);
        if (validInput)
        {
            bool inRange = inputIndex < modelBuilder.matrixList.Count;
            if (inRange)
            {
                FindOffsetVectors(inputIndex);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Matrix index out of range");
            }
        }
        else
        {
            throw new ArgumentException("Invalid matrix index exception");
        }
    }
}
