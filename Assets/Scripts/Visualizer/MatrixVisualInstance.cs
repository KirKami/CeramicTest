using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Visualiziation of Transform matrix
/// </summary>
public class MatrixVisualInstance : MonoBehaviour
{
    //Renderer component to control color
    public Renderer objectRenderer;

    //Animation Sequence
    public Sequence translationSequence;

    /// <summary>
    /// Dynamically creates new graphical instance of matrix object
    /// </summary>
    /// <param name="transformMatrix">TRS of this object</param>
    /// <param name="instanceColor">Color of object</param>
    /// <returns></returns>
    public static MatrixVisualInstance CreateNewInstance(Matrix4x4 transformMatrix, Color instanceColor)
    {
        //Create Cube
        var newObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //Apply Transform Matrix to cube
        newObject.transform.position = transformMatrix.GetPosition();
        newObject.transform.rotation = transformMatrix.rotation;
        newObject.transform.localScale = transformMatrix.lossyScale;

        //Set up visual components
        var newInstance = newObject.AddComponent<MatrixVisualInstance>();
        newInstance.objectRenderer = newObject.GetComponent<Renderer>();
        newInstance.objectRenderer.material.color = instanceColor;

        return newInstance;
    }
    /// <summary>
    /// Animation sequence that moves cube to using matrix and vectors to objects in space.
    /// </summary>
    /// <param name="matrix">Base transform matrix</param>
    /// <param name="offsets">Offset vectors to all objects in space</param>
    public void StartTranslationSequence(Matrix4x4 matrix, List<Vector3> offsets)
    {
        //Kill existing animation sequence
        if(translationSequence != null) translationSequence.Kill();

        //Reset position to base
        transform.position = matrix.GetPosition();

        //Initialize new sequence
        translationSequence = DOTween.Sequence();

        //Create commands in sequence for each offset vector
        foreach(Vector3 offset in offsets)
        {
            //Calculate resulting position in world space
            Vector3 resultPosition = matrix.GetPosition() + offset;
            //Add command to move cube to this position in 1 second and wait for 1 second
            translationSequence.Append(transform.DOMove(resultPosition, 1f));
            translationSequence.AppendInterval(1f);
        }
        //Start sequence
        translationSequence.Play();
    }
    public void StartTranslationSequence(Matrix4x4 matrix, Matrix4x4[] spaceMatrices, int matrixIndexOffset)
    {
        //Kill existing animation sequence
        if (translationSequence != null) translationSequence.Kill();

        //Reset position to base
        transform.position = matrix.GetPosition();

        //Initialize new sequence
        translationSequence = DOTween.Sequence();

        foreach (var item in spaceMatrices)
        {
            translationSequence.Append(transform.DOMove(item.GetPosition(), 1f));
            translationSequence.Join(transform.DORotate(item.rotation.eulerAngles, 1f));
            translationSequence.Join(transform.DOScale(item.lossyScale, 1f));
            translationSequence.AppendInterval(1f);
        }

        translationSequence.Goto(matrixIndexOffset * 2);

        //Start sequence
        translationSequence.Play();
    }
}
