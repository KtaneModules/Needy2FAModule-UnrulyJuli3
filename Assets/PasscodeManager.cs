using KeepCoding;
using System;
using System.Text;
using MonoRand = KeepCoding.MonoRandom;

public class PasscodeManager : CacheableBehaviour
{
	public const int PasscodeLength = 6;
	public const double ResetTime = 15 * 1000;

	public const float NeedyTimeBuffer = 1;

	private static readonly DateTime s_origin = DateTime.MinValue.AddYears(1969);

	public string CurrentCode { get; private set; }

	public event Action<string> OnRegenerate;

	private double _time;
	private int _lastSeed;

	public double TimeSinceReset
	{
		get
		{
			return _time % ResetTime;
		}
	}

	public double TimeUntilReset
	{
		get
		{
			return ResetTime - TimeSinceReset;
		}
	}

	private int CurrentSeed
	{
		get
		{
			return (int)(_time / ResetTime);
		}
	}

	private string CreatePasscode(MonoRand rnd)
	{
		StringBuilder code = new StringBuilder();
		for (int i = 0; i < PasscodeLength; i++)
			code.Append(rnd.Next(0, 10));

		return code.ToString();
	}

	private void Regenerate()
	{
		string code = CreatePasscode(new MonoRand(CurrentSeed));

		if (OnRegenerate != null)
			OnRegenerate.Invoke(code);

		CurrentCode = code;
	}

	private void Update()
	{
		_time = DateTime.UtcNow.Subtract(s_origin).TotalMilliseconds;

		if (CurrentSeed != _lastSeed)
		{
			_lastSeed = CurrentSeed;
			Regenerate();
		}
	}
}
