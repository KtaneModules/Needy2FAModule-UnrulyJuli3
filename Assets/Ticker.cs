using KeepCoding;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Ticker : CacheableBehaviour
{
	[SerializeField]
	private Button _buttonUp;

	[SerializeField]
	private Button _buttonDown;

	[SerializeField]
	private Button _numberButton;

	[SerializeField]
	private Image _numberImage;

	private static Color s_backingColor = new Color(0, 0, 0, 0.5f);

	private static ReadOnlyCollection<int> s_numberRange = Enumerable.Range(0, 10).ToReadOnly();

	private bool _isSelected;

	public bool IsSelected
	{
		get
		{
			return _isSelected;
		}
		set
		{
			_isSelected = value;
			SetSelected();
		}
	}

	public bool IsEnabled { get; set; }

	public int Value
	{
		get
		{
			return s_numberRange.ElementAtWrap(_position);
		}
		set
		{
			_position = s_numberRange.IndexOf(value);
			SetDisplay();
		}
	}

	private int _position;

	public event Action OnValueChanged;

	public event Action OnSelected;

	private void Awake()
	{
		IsSelected = false;
		_buttonUp.onClick.AddListener(() => Navigate(1));
		_buttonDown.onClick.AddListener(() => Navigate(-1));
		_numberButton.onClick.AddListener(Select);
	}

	private void SetSelected()
	{
		_numberImage.color = IsSelected ? Color.white : s_backingColor;
		GetChild<Text>().color = IsSelected ? Color.black : Color.white;
	}

	private void Navigate(int offset)
	{
		_position += offset;
		SetDisplay();

		if (OnValueChanged != null)
			OnValueChanged.Invoke();
	}

	private void Select()
	{
		if (IsSelected)
			return;

		if (OnSelected != null)
			OnSelected.Invoke();
	}

	public void SetDisplay()
	{
		GetChild<Text>().text = IsEnabled ? Value.ToString() : "-";
	}
}
