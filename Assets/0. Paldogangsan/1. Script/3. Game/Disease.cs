using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Disease
{
    public string diseaseName;
    public List<Material> goodFood;
    public List<Material> badFood;

    public Disease Clone()
    {
        Disease temp = new Disease();
        temp.diseaseName = this.diseaseName;

        Material[] gFood = (Material[])this.goodFood.ToArray().Clone();
        temp.goodFood = gFood.ToList();

        Material[] bFood = (Material[])this.badFood.ToArray().Clone();
        temp.badFood = bFood.ToList();

        return temp;
    }
};