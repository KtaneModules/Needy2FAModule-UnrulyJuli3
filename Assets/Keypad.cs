using KeepCoding;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class Keypad : CacheableBehaviour
{
	[SerializeField]
	private Ticker _ticker;

	private List<Ticker> _tickers;

	private static ReadOnlyCollection<KeyCode> s_keyCodes = new[]
	{
		KeyCode.Alpha0,
		KeyCode.Alpha1,
		KeyCode.Alpha2,
		KeyCode.Alpha3,
		KeyCode.Alpha4,
		KeyCode.Alpha5,
		KeyCode.Alpha6,
		KeyCode.Alpha7,
		KeyCode.Alpha8,
		KeyCode.Alpha9
	}.ToReadOnly();

	public string InputCode
	{
		get
		{
			return _tickers.Select(t => t.Value).Join("");
		}
	}

	private bool _isEnabled;

	private void Start()
	{
		CreateTickers();
		SetEnabled(false);
	}

	private void CreateTickers()
	{
		_tickers = new List<Ticker>();
		for (int i = 0; i < PasscodeManager.PasscodeLength; i++)
			_tickers.Add(CreateTicker(i));
	}

	public void SetEnabled(bool enabled)
	{
		_isEnabled = enabled;

		foreach (Ticker ticker in _tickers)
		{
			ticker.IsEnabled = _isEnabled;
			ticker.IsSelected = false;
			ticker.Value = 0;
		}

		if (_isEnabled)
			_tickers.First().IsSelected = true;
	}
	
	private Ticker CreateTicker(int i)
	{
		Ticker ticker = Instantiate(_ticker, transform);
		ticker.OnSelected += () => SetSelected(i);
		return ticker;
	}

	private void SetSelected(int n)
	{
		if (!_isEnabled)
			return;

		for (int i = 0; i < _tickers.Count; i++)
			_tickers[i].IsSelected = i == n;
	}

	private void NavigateSelected(int offset)
	{
		SetSelected((_tickers.FindIndex(t => t.IsSelected) + offset).Modulo(_tickers.Count));
	}

	public void SetValue(int i, int n)
	{
		_tickers[i].Value = n;
		SetSelected(i);
	}

	private void Update()
	{
		if (!_isEnabled || !GetParent<Needy2FA>().IsSelected)
			return;

		for (int n = 0; n < s_keyCodes.Count; n++)
		{
			if (Input.GetKeyDown(s_keyCodes[n]))
			{
				_tickers.First(t => t.IsSelected).Value = n;
				NavigateSelected(1);
				break;
			}
		}

		if (Input.GetKeyDown(KeyCode.Backspace))
			NavigateSelected(-1);
	}
}
