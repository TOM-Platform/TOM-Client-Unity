using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SpaceConverterTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void NewTestScriptSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    //[Test]
    //public void GetWorldCoordinates_CalculatesCorrectCoordinates()
    //{
    //    // Arrange
    //    Vector3 centerPosition = Vector3.zero;
    //    Quaternion centerRotation = Quaternion.identity;
    //    float width = 1f;
    //    float height = 1f;

    //    // Act
    //    Vector3 topLeftWorld = SpaceConverter.GetWorldCoordinates(centerPosition, centerRotation, SpaceConverter.CornerPosition.TOP_LEFT, width, height);
    //    Vector3 topRightWorld = SpaceConverter.GetWorldCoordinates(centerPosition, centerRotation, SpaceConverter.CornerPosition.TOP_RIGHT, width, height);
    //    Vector3 bottomLeftWorld = SpaceConverter.GetWorldCoordinates(centerPosition, centerRotation, SpaceConverter.CornerPosition.BOTTOM_LEFT, width, height);
    //    Vector3 bottomRightWorld = SpaceConverter.GetWorldCoordinates(centerPosition, centerRotation, SpaceConverter.CornerPosition.BOTTOM_RIGHT, width, height);

    //    // Assert
    //    Assert.AreEqual(new Vector3(-0.5f, 0.5f, 0f), topLeftWorld);
    //    Assert.AreEqual(new Vector3(0.5f, 0.5f, 0f), topRightWorld);
    //    Assert.AreEqual(new Vector3(-0.5f, -0.5f, 0f), bottomLeftWorld);
    //    Assert.AreEqual(new Vector3(0.5f, -0.5f, 0f), bottomRightWorld);
    //}
}
