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
            if (GameSceneManager.instance.getGameInfoUI.EnablePurchase())
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
        if (col.CompareTag("Player") && GameSceneManager.instance.getGameInfoUI.EnablePurchase())
        {
            //PlayerController의 손동작을 활성화한다.
            col.gameObject.GetComponentInChildren<PlayerController>().isActivedHand = true;

            SoundManager.instance.PlayClip(biggerClip, false, 1.0f);

            if (!GameSceneManager.instance.getGameInfoUI.CheckSameFood
                (foodImage.gameObject.GetComponent<Renderer>().material))
            {
                touchOn = true;
                foodImage.transform.localScale += defaultlocalScale * 0.75f;
                imageMaterial.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 0.5f));
            }
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
        if (FoodManager.instance.CheckFood(foodImage.gameObject.GetComponent<Renderer>().material))
        {
            SoundManager.instance.PlayClip(correctClip);
            GameSceneManager.instance.getGameInfoUI.RightPurchase(foodImage.gameObject.GetComponent<Renderer>().material);
            FoodManager.instance.goodCount++;
        }
        else
        {
            SoundManager.instance.PlayClip(incorrectClip);
            GameSceneManager.instance.getGameInfoUI.WrongPurchase(foodImage.gameObject.GetComponent<Renderer>().material);
            FoodManager.instance.badCount++;
        }
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
}
