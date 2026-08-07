using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DiscriptionFresher : MonoBehaviour
{
    [SerializeField] private DiscriptionView discriptionView;
    [SerializeField] private ScrollRect conditionView;//条件
    [SerializeField] private ConditionItem conditionItemPrefab;

    public void FreshDisplay()
    {
        //更新数据刷新显示
        discriptionView.GenerateID = MakeFunctioner.instance.GenerateID;
        discriptionView?.GetComponent<DiscriptionViewSetter>()?.FreshDisplay();
        //清空条件列表
        foreach (Transform child in conditionView.content)//清空条件列表
        {
            Destroy(child.gameObject);
        }
        //依据当前的条件加入表项
        MakeFunctioner.instance.FreshRequireList();
        Dictionary<int,int> requireList = MakeFunctioner.instance.RequireList;
        int makeNumber = MakeFunctioner.instance.MakeNumber;
        foreach(KeyValuePair<int,int> item in requireList)
        {
            int requireItemID = item.Key;
            int requireItemNumber = item.Value * makeNumber;
            ConditionItem conditionItem = Instantiate(conditionItemPrefab,conditionView.content.transform);
            conditionItem.RequireItemID = requireItemID;
            conditionItem.RequireItemNumber = requireItemNumber;
            conditionItem.GetComponent<ConditionItemViewSetter>()?.FreshDisplay();
        }
    }

}
