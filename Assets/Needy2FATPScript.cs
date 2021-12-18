using KeepCoding;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Needy2FATPScript : TPScript<Needy2FA>
{
	private IEnumerator SetValue(int[] numbers)
	{
		for (int i = 0; i < numbers.Length; i++)
		{
			GetChild<Keypad>().SetValue(i, numbers[i]);
			yield return new WaitForSeconds(0.05f);
		}
	}

	private int[] GetNumbers(string input)
	{
		return input.Where(char.IsDigit).Select(e => (int)e.ToNumber()).ToArray();
	}

	public override IEnumerator ForceSolve()
	{
		yield return SetValue(GetNumbers(Get<PasscodeManager>().CurrentCode));
	}

	public override IEnumerator Process(string command)
	{
		int[] numbers = GetNumbers(command);
		if (numbers.Length != PasscodeManager.PasscodeLength)
		{
			yield return SendToChatError(numbers.Length < PasscodeManager.PasscodeLength ? "Not enough digits!" : "Too many digits!");
			yield break;
		}

		yield return SetValue(numbers);
	}
}
