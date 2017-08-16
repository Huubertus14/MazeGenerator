using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSettings : MonoBehaviour {

    


        //Change y pos
    //Slider vars
    //Get the values of the sliders to see the size of the maze
    private Slider xSlider;
    private Slider ySlider;

    public int xSize;
    public int ySize;

    Camera cam;

    public float zoomX = 59f;
    public float zoomY  = 86f;

    // Use this for initialization
    void Start () {
        cam = Camera.main;
        xSlider = GameObject.FindGameObjectWithTag("xSlider").GetComponent<Slider>();
        ySlider = GameObject.FindGameObjectWithTag("ySlider").GetComponent<Slider>();
        cam.transform.position = new Vector3(0,4.93f,0);
    }
	
	public void SetCameraPOV()
    {
        xSize = (int)xSlider.value;
        ySize = (int)ySlider.value;

        if(xSize > ySize && ySize < 13)
        {
            //Always need to check the highest value
            ScaleCameraX(xSize);
        }
        else
        {
            ScaleCameraY(ySize);
        }

    }

    private void ScaleCameraX(int value)
    {
        //The value can be from 5 to 25
        //So we need te scale between them

        //Min 69 max 119

        cam.fieldOfView = zoomX +((value/2)*5);

    }

    private void ScaleCameraY(int value)
    {
        //The value can be from 5 to 25
        //So we need te scale between them

        //Max 144
        //Min 69
        cam.fieldOfView = zoomY + ((value / 2) * 5);
    }

}
