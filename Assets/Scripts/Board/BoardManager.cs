using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;


public class BoardManager : MonoBehaviour
{
    public int maxItems = 2;

    public bool carPicked = false;
    public bool gunPicked = false;
    public bool heliPicked = false;
    public bool magnetPicked = false;

    //General
    public GameObject board;
    private bool isActive;
    public Controller controller;

    //Right Part
    public Action currentAction = null;
    public string ButtonName = null;


    //Left Part
    public GameObject leftSlot;
    public GameObject leftItem;
    public GameObject rightSlot;
    public GameObject rightItem;
    public GameObject upSlot;
    public GameObject upItem;
    public GameObject downSlot;
    public GameObject downItem;
    public GameObject chosenItem;
    public HashSet<string> activeItems = new HashSet<string>();
    public string chosenSlot = "";

    private void Awake()
    {
        isActive = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InteractWithBoard();
        }
    }
    public GameObject carB;
    public GameObject gunB;
    public GameObject magnetB;
    public GameObject heliB;
    void InteractWithBoard()
    {
        board.SetActive(!isActive);
        carB.SetActive(carPicked);
        magnetB.SetActive(magnetPicked);
        heliB.SetActive(heliPicked);
        gunB.SetActive(gunPicked);
        isActive = !isActive;
    }

    public void ChangeButtonAction()
    {
        switch (ButtonName)
        {
            case "A":
                controller.keyAAction = currentAction;
                break;
            case "W":
                controller.keyWAction = currentAction;
                break;
            case "S":
                controller.keySAction = currentAction;
                break;
            case "D":
                controller.keyDAction = currentAction;
                break;
        }
        ButtonName = "";
        currentAction = null;
    }

    public void choseCurrentAction(string Action)
    {
        switch (Action)
        {
            case "RollRight":
                currentAction = controller.RollRight;
                break;
            case "RollLeft":
                currentAction = controller.RollLeft;
                break;
        }
        if (ButtonName != "")
        {
            Debug.Log("test");
            ChangeButtonAction();
        }
    }

    public void choseButtonName(string _ButtonName)
    {
        ButtonName = _ButtonName;
        if (currentAction != null)
        {
            ChangeButtonAction();
        }
    }

    void equipItem(GameObject myItem, string slot)
    {
        if (Itemcount() > maxItems - 1) return;
        if (activeItems.Contains(myItem.name)) return;
        activeItems.Add(myItem.name);


        GameObject newObj = Instantiate(myItem);
        newObj.transform.localScale = Vector3.zero;
        StartCoroutine(ScaleIn(newObj, 0.2f));
        newObj.transform.SetParent(controller.transform);
        Vector3 newCord = Vector3.zero;
        Quaternion newRotation = Quaternion.identity;
        if (slot == "up")
        {
            if (upItem != null) removeItem(upItem);
            newCord.y = myItem.GetComponent<itemHandler>().distance;
            newRotation = Quaternion.Euler(0, 0, myItem.GetComponent<itemHandler>().startRotation - 180);
            upItem = newObj;
            newObj.transform.localPosition = newCord;
            newObj.transform.localRotation = newRotation;
            if (Itemcount() > maxItems)
            {
                StopAllCoroutines();
                Destroy(upItem);
                return;
            }
        }
        if (slot == "down")
        {
            if (downItem != null) removeItem(downItem);
            newCord.y = -myItem.GetComponent<itemHandler>().distance;
            newRotation = Quaternion.Euler(0, 0, myItem.GetComponent<itemHandler>().startRotation);
            downItem = newObj;
            newObj.transform.localPosition = newCord;
            newObj.transform.localRotation = newRotation;
            if (Itemcount() > maxItems) { StopAllCoroutines(); Destroy(downItem); return; }
        }
        if (slot == "right")
        {
            if (rightItem != null) removeItem(rightItem);
            newCord.x = myItem.GetComponent<itemHandler>().distance;
            newRotation = Quaternion.Euler(0, 0, myItem.GetComponent<itemHandler>().startRotation - 270);
            rightItem = newObj;
            newObj.transform.localPosition = newCord;
            newObj.transform.localRotation = newRotation;
            if (Itemcount() > maxItems) { StopAllCoroutines(); Destroy(rightItem); return; }
        }
        if (slot == "left")
        {
            if (leftItem != null) removeItem(leftItem);
            newCord.x = -myItem.GetComponent<itemHandler>().distance;
            newRotation = Quaternion.Euler(0, 0, myItem.GetComponent<itemHandler>().startRotation - 90);
            leftItem = newObj;
            newObj.transform.localPosition = newCord;
            newObj.transform.localRotation = newRotation;
            if (Itemcount() > maxItems) { StopAllCoroutines(); Destroy(leftItem); return; }
        }

        if (myItem.name == "Car") handleCar(newObj);
        if (myItem.name == "Gun") handleGun(newObj);
        if (myItem.name == "Magnet") handleMagnet(newObj);
        if (myItem.name == "Heli") handleHeli(newObj);
        

    }

    void handleGun(GameObject obj)
    {
        controller.canShoot = true;
        controller.firePoint = obj.GetComponent<itemHandler>().gunF1.transform;
        controller.gun = obj;

    }
    void removeItem(GameObject _item)
    {
        if (_item == null) return;
        if (!activeItems.Contains(_item.name[..^7])) return;
        activeItems.Remove(_item.name[..^7]);
        Debug.Log(_item.name[..^7]);
        if (_item.name[..^7] == "Car") { Destroy(_item); controller.canCarMove = false; }
        if (_item.name[..^7] == "Gun") { Destroy(_item); controller.canShoot = false; }
        if (_item.name[..^7] == "Magnet") { Destroy(_item); controller.canMagnet = false; }
        if (_item.name[..^7] == "Heli") { Destroy(_item); controller.hasHeli = false; }
        

    }

    public void choseItem(GameObject _item)
    {
        chosenItem = _item;
        if (chosenSlot != "")
        {
            equipItem(chosenItem, chosenSlot);
            chosenItem = null;
            chosenSlot = "";
        }
    }

    void handleMagnet(GameObject magnet)
    {
        controller.magnetHead = magnet.GetComponent<itemHandler>().magnetH1;
        controller.canMagnet = true;
    }

    void handleCar(GameObject car)
    {
        controller.carGroundCheck1 = car.GetComponent<itemHandler>().carG1.transform;
        controller.carGroundCheck2 = car.GetComponent<itemHandler>().carG2.transform;
        controller.canCarMove = true;
    }
    void handleHeli(GameObject Heli)
    {
        controller.hasHeli = true;
        controller.heli = Heli;
    }

    public void choseSlot(string _slot)
    {
        if (chosenSlot == _slot)
        {
            switch (chosenSlot)
            {
                case "up":
                    removeItem(upItem);
                    Destroy(topImage);
                    upItem = null;
                    break;
                case "down":
                    removeItem(downItem);
                    Destroy(downImage);
                    downItem = null;
                    break;
                case "left":
                    removeItem(leftItem);
                    Destroy(leftImage);
                    leftItem = null;    
                    break;
                case "right":
                    removeItem(rightItem);
                    Destroy(rightImage);  
                    rightItem = null;
                    break;
            }
            chosenSlot = "";
            Itemcount();
            return;
        }
        chosenSlot = _slot;
        if (chosenItem != null)
        {
            equipItem(chosenItem, chosenSlot);
            chosenItem = null;
            chosenSlot = "";
        }
    }

    public GameObject tick1;
    public GameObject tick2;
    public GameObject[] battery;

    public int Itemcount()
    {
        int count = 0;
        if (leftItem != null) count++;
        if (rightItem != null) count++;
        if (upItem != null) count++;
        if (downItem != null) count++;
        tick1.SetActive(false);
        tick2.SetActive(false);
        switch (count)
        {
            case 0:
                foreach (GameObject item in battery)
                {
                    item.GetComponent<Image>().color = Color.white;
                }
                break;
            case 1:
                tick1.SetActive(true);
                foreach (GameObject item in battery)
                {
                    item.GetComponent<Image>().color = Color.yellow;
                }
                break;
            case 2:
                tick2.SetActive(true);
                tick1.SetActive(true);
                foreach (GameObject item in battery)
                {
                    item.GetComponent<Image>().color = Color.red;
                }
                break;
        }
        if(maxItems >= count)updateSlots();
        return count;
    }

    IEnumerator ScaleIn(GameObject obj, float duration)
    {
        Vector3 startScale = Vector3.zero;
        Vector3 targetScale = Vector3.one;
        switch (obj.name[..^7])
        {
            case "Gun":
                targetScale = Vector3.one * 0.5f;
                break;
            case "Magnet":
                targetScale = Vector3.one * 0.5f;
                break;
            case "Car":
                targetScale = Vector3.one * 1f;
                break;
            case "Heli":
                targetScale = Vector3.one;
                break;
        }
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            obj.transform.localScale = Vector3.Lerp(startScale, targetScale, t / duration);
            yield return null;
        }

        obj.transform.localScale = targetScale;
    }


    public GameObject carImg;
    public GameObject heliImg;
    public GameObject gunImg;
    public GameObject magnetImg;


    public GameObject leftImage;
    public GameObject rightImage;
    public GameObject topImage;
    public GameObject downImage;
    void updateSlots()
    {
        if(topImage!= null) Destroy(topImage);
        if (leftImage != null) Destroy(leftImage);
        if (rightImage != null) Destroy(rightImage);
        if (downImage != null) Destroy(downImage);
        topImage = null;
        leftImage = null;
        rightImage = null;
        downImage = null;
        if (leftItem != null)
        {
            switch (leftItem.name[..^7])
            {
                case "Car":
                    leftImage = Instantiate(carImg, leftSlot.transform);
                    break;
                case "Magnet":
                    leftImage = Instantiate(magnetImg, leftSlot.transform);
                    break;
                case "Heli":
                    leftImage = Instantiate(heliImg, leftSlot.transform);
                    break;
                case "Gun":
                    leftImage = Instantiate(gunImg, leftSlot.transform);
                    break;
            }
        }
        if (rightItem != null)
        {
            switch (rightItem.name[..^7])
            {
                case "Car":
                    rightImage = Instantiate(carImg, rightSlot.transform);
                    break;
                case "Magnet":
                    rightImage = Instantiate(magnetImg, rightSlot.transform);
                    break;
                case "Heli":
                    rightImage = Instantiate(heliImg, rightSlot.transform);
                    break;
                case "Gun":
                    rightImage = Instantiate(gunImg, rightSlot.transform);
                    break;
            }
        }
        if (upItem != null)
        {

            switch (upItem.name[..^7])
            {
                case "Car":
                    topImage = Instantiate(carImg, upSlot.transform);
                    break;
                case "Magnet":
                    topImage = Instantiate(magnetImg, upSlot.transform);
                    break;
                case "Heli":
                    topImage = Instantiate(heliImg, upSlot.transform);
                    break;
                case "Gun":
                    topImage = Instantiate(gunImg, upSlot.transform);
                    break;
            }
        }
        if (downItem != null)
        {
            switch (downItem.name[..^7])
            {
                case "Car":
                    downImage = Instantiate(carImg, downSlot.transform);
                    break;
                case "Magnet":
                    downImage = Instantiate(magnetImg, downSlot.transform);
                    break;
                case "Heli":
                    downImage = Instantiate(heliImg, downSlot.transform);
                    break;
                case "Gun":
                    downImage = Instantiate(gunImg, downSlot.transform);
                    break;
            }
        }
    }
}
