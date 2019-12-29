using UnityEngine;
using System.Collections;

public class FoodArea : MonoBehaviour
{
    public Transform foodImage;
    Material imageMaterial;
    
    public Transform player;

    private Vector3 defaultlocalScale;

    private bool touchOn;
    private bool isCheck;

    [Header("Sound")]
    [SerializeField] private AudioClip biggerClip;
    [SerializeField] private AudioClip correctClip;
    [SerializeField] private AudioClip incorrectClip;

    /// <summary>
    /// *문제점 : Start함수로 했을 경우 Default-Material이 나타나는 현상이 생김.
    /// FoodManager의 Start함수와 FoodArea함수가 동시에 호출되는 시점에서 문제가 발생되었다.
    /// 따라서, 셋팅부분을 FoodManager에서 호출할 수 있도록 위임해준다.
    /// </summary>
    public void InitialSetting()
    {
        imageMaterial = foodImage.GetComponent<Renderer>().material;
        imageMaterial.name = imageMaterial.name.Remove(imageMaterial.name.IndexOf(' '));
        imageMaterial.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 0.1f));

        if (FoodManager.stage != 3)
            defaultlocalScale = new Vector3(1f, 1f, 1f);
        else
            defaultlocalScale = new Vector3(0.1f, 0.1f, 0.1f);

        foodImage.transform.localScale = defaultlocalScale;
    }
   
    void Update()
    {
        if (touchOn)
        {
            if (FoodUI.instance.curIndex < FoodUI.instance.maximum_Amount)
            {
                if (Check_CanBuyFood() && InputSystem.instance.curHandState == InputSystem.HandState.Left ||
                    !Check_CanBuyFood() && InputSystem.instance.curHandState == InputSystem.HandState.Right)
                {
                    BuyFood();

                    foodImage.transform.gameObject.SetActive(false);
                    this.gameObject.SetActive(false);

                    touchOn = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && FoodUI.instance.curIndex < FoodUI.instance.maximum_Amount)
        {
            //PlayerController의 손동작을 활성화한다.
            col.gameObject.GetComponentInChildren<PlayerController>().isActivedHand = true;

            SoundManager.instance.PlayClip(biggerClip, false, 1.0f);

            CheckSameFood();
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Controller.tutofoodarea = false; //?

            //PlayerController의 손동작을 비활성시킨다.
            col.gameObject.GetComponentInChildren<PlayerController>().isActivedHand = false;

            foodImage.transform.gameObject.SetActive(false);
            this.gameObject.SetActive(false);

            touchOn = false;
        }
    }

    void BuyFood()
    {
        FoodUI foodUI = FoodUI.instance;

        foodUI.foodArray[foodUI.curIndex] = foodImage.gameObject.GetComponent<Renderer>().material;
        foodUI.foodArray[foodUI.curIndex].name = foodImage.gameObject.GetComponent<Renderer>().material.name;
        foodUI.foods[foodUI.curIndex].SetActive(true);
        foodUI.foods[foodUI.curIndex].GetComponent<GUITexture>().texture = foodUI.foodArray[foodUI.curIndex].mainTexture;
        foodUI.foods[foodUI.curIndex].gameObject.transform.localScale = new Vector3(0.09f * 0.75f, 0.16f * 0.75f, 1);
        if (FoodManager.instance.CheckFood(FoodUI.instance.foodArray[foodUI.curIndex]))
        {
            //FoodManager.goodFood++;
            SoundManager.instance.PlayClip(correctClip);
            foodUI.foods[foodUI.curIndex].GetComponent<GUITexture>().color = new Color(0.192f, 0.454f, 0.729f);
            FoodManager.instance.goodCount++;
        }
        else
        {
            //FoodManager.badFood++;
            SoundManager.instance.PlayClip(incorrectClip);
            foodUI.foods[foodUI.curIndex].GetComponent<GUITexture>().color = new Color(0.745f, 0.356f, 0.356f);
            FoodManager.instance.badCount++;
        }

        FoodUI.instance.curIndex++;
    }

    bool Check_CanBuyFood()
    {
        Vector3 gapVec = new Vector3(foodImage.position.x - player.position.x, foodImage.position.y - player.position.y, foodImage.position.z - player.position.z);
        Vector3 vec = new Vector3(gapVec.x, 0, gapVec.z);
        float angle = Vector3.Angle(vec, player.right);

        if (angle > 90)
            return true;
        else
            return false;
    }

    void CheckSameFood()
    {
        bool isSame = false;
        if (FoodUI.instance.foodArray[0] == null)
        {
            foodImage.transform.localScale += defaultlocalScale * 0.75f;
            imageMaterial.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 0.5f));
            touchOn = true;
        }

        else
        {
            for (int i = 0; i < FoodUI.instance.curIndex; i++)
            {
                if (FoodUI.instance.foodArray[i].name == foodImage.gameObject.GetComponent<Renderer>().materials[0].name)
                {
                    isSame = true;
                    break;
                }
                else
                {
                    isSame = false;
                }
            }

            if (!isSame)
            {
                touchOn = true;
                foodImage.transform.localScale += defaultlocalScale * 0.75f;
                imageMaterial.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 0.5f));
            }
        }
    }      
}
