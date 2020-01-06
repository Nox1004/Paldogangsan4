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
    /// *������ : Start�Լ��� ���� ��� Default-Material�� ��Ÿ���� ������ ����.
    /// FoodManager�� Start�Լ��� FoodArea�Լ��� ���ÿ� ȣ��Ǵ� �������� ������ �߻��Ǿ���.
    /// ����, ���úκ��� FoodManager���� ȣ���� �� �ֵ��� �������ش�.
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
            //PlayerController�� �յ����� Ȱ��ȭ�Ѵ�.
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

            //PlayerController�� �յ����� ��Ȱ����Ų��.
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
