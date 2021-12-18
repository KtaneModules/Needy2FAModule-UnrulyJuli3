using KeepCoding;
using UnityEngine;
using UnityEngine.UI;

public class Timer : CacheableBehaviour
{
	public void SetTime(float time, float limit)
	{
		Get<Image>().fillAmount = time / limit;
		GetChild<Text>().text = Mathf.Ceil(time / 1000f).ToString();
	}
}
