using KeepCoding;
using UnityEngine;
using UnityEngine.UI;

public class Needy2FA : ModuleScript
{
	public bool IsSelected { get; private set; }

	private void Start()
	{
		Get<KMSelectable>().Assign(
			onInteract: () => SetFocus(true),
			onDefocus: () => SetFocus(false)
		);

		Get<PasscodeManager>().OnRegenerate += Regenerate;

		//Assign(needyTimerExpired: TimeExpired);
		Get<KMNeedyModule>().OnTimerExpired += TimeExpired;
	}

	private void SetFocus(bool selected)
	{
		IsSelected = selected;
		GetChild<GraphicRaycaster>().enabled = IsSelected;
	}

	/* private void SetTime(double time)
	{
		Get<KMNeedyModule>().SetNeedyTimeRemaining(Mathf.Ceil((float)time / 1000));
	} */

	public override void OnNeedyActivate()
	{
		Log("Activated, current code is {0}", Get<PasscodeManager>().CurrentCode);

		GetChild<Keypad>().SetEnabled(true);

		Get<KMNeedyModule>().SetNeedyTimeRemaining((float)(PasscodeManager.ResetTime + Get<PasscodeManager>().TimeUntilReset) / 1000 - PasscodeManager.NeedyTimeBuffer);
	}

	public override void OnNeedyDeactivate()
	{
		//_canEndTimer = false;
		//this.Stop(_activationRoutine);
		GetChild<Keypad>().SetEnabled(false);
	}

	private void Regenerate(string code)
	{
		if (IsNeedyActive)
			Log("Code changed to {0}", code);
	}

	private void TimeExpired()
	{
		Log("Time expired, submitted code is {0}", GetChild<Keypad>().InputCode);

		if (GetChild<Keypad>().InputCode == Get<PasscodeManager>().CurrentCode)
			Solve("Successfully deactivated");

		else
			Strike("Strike!");

		OnNeedyDeactivate();
	}

	private void Update()
	{
		GetChild<Timer>().SetTime((float)Get<PasscodeManager>().TimeUntilReset, (float)PasscodeManager.ResetTime);
	}
}
