using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;



public class GameManager : MonoBehaviour 
{
	System.Random rand = new System.Random();

	string verySimpleAdLink = "";

	public Color red = Color.red;
	public Color blue = Color.blue;
	public Color green = Color.green;
	public Color yellow = Color.yellow;

	public float speedMinInSeconds = 0.3f;
	public float speedMaxInSeconds = 1.5f;

	public DColor[] colors;

	public int numberOfPlayToShowInterstitial = 20;

	public Text textScore;

	public Image title;
	public Image instruction;

	public Transform dotPrefab;

	int _point = 0;
	int point
	{
		get
		{
			return _point;
		}

		set
		{
			_point = value;

			textScore.text = _point.ToString();
		}
	}

	public AudioClip[] pocs;
	public AudioClip lose;

	void Awake()
	{
		Application.targetFrameRate = 60;

		ResetUIElement();

	

		transform.position = Vector3.zero;
	}

	void PlaySoundPoc()
	{
		int i = rand.Next(0,pocs.Length);
		GetComponent<AudioSource>().PlayOneShot(pocs[i]);
	}

	void PlaySoundLose()
	{
		GetComponent<AudioSource>().PlayOneShot(lose);
	}

	void ResetUIElement()
	{
		Color c = title.color;
		c.a = 1;
		title.color = c;

		c = instruction.color;
		c.a = 0;
		instruction.color = c;

		c = textScore.color;
		c.a = 0;
		textScore.color = c;


	
		textScore.text = "TAP TO BEGIN";
		textScore.DOFade(0.5f,1)
			.OnComplete(() => {
				InputTouch.OnTouched += OnTouched;
			});

	}

	void OnTouched (TouchDirection td)
	{
		DOStart();
	}

	public void DOStart()
	{
		InputTouch.OnTouched -= OnTouched;

		FindObjectOfType<Floor>().DOEnable();

		DOCreateDot();

		title.DOKill();
		instruction.DOKill();
		textScore.DOKill();

		title.DOFade(0,1);
		instruction.DOFade(1,1).OnComplete(() => {
			instruction.DOFade(0,1).SetDelay(3);
		});

		textScore.DOFade(0.0f,1).OnComplete(()=>{
			point = 0;
			textScore.DOFade(0.5f,1);
		});
	}


	void DOCreateDot()
	{
		var inst = Instantiate(dotPrefab) as Transform;

		inst.parent = transform;

		inst.GetComponent<DBase>().SetColor(colors[rand.Next(0,colors.Length)]);

		inst.transform.position = new Vector3(FindObjectOfType<Floor>().GetPositionForDot(), 2f * Camera.main.orthographicSize, 0);

		Invoke("DOCreateDot",UnityEngine.Random.Range(speedMinInSeconds,speedMaxInSeconds));
	}

	public void GameOver()
	{
		ShowAds();

		PlaySoundLose();

		Utils.SetBest(point);

		var dots = FindObjectsOfType<Dot>();

		foreach(var d in dots)
		{
			if(d != null && d.gameObject != null)
				Destroy(d.gameObject);
//			d.sr.DOFade(0,0.3f).OnComplete(() => {
//				DOVirtual.DelayedCall(0.01f, () => {
//					Destroy(d.gameObject);
//				});
//
//			});
		}

		CancelInvoke("DOCreateDot");

		FindObjectOfType<Floor>().DODisable();

		title.DOKill();
		instruction.DOKill();
		textScore.DOKill();

		title.DOFade(1,1);
		instruction.DOFade(0,1);

		textScore.DOFade(0.0f,1)
			.OnComplete( () => {
				textScore.text = "BEST " + Utils.GetBest();
				textScore.DOFade(0.5f,1);
			});

		DOVirtual.DelayedCall(3,() => {
			




			InputTouch.OnTouched += OnTouched;
		});
	}
		

	public void Add1Point()
	{
		point++;

		PlaySoundPoc();
	}





	public void ShowAds()
	{
		int count = PlayerPrefs.GetInt("GAMEOVER_COUNT",0);
		count++;

		#if APPADVISORY_ADS
		if(count > numberOfPlayToShowInterstitial && AdsManager.instance.IsReadyInterstitial())
		{
		PlayerPrefs.SetInt("COUNT_ADS",0);
		AdsManager.instance.ShowInterstitial();
		}
		else
		{
		PlayerPrefs.SetInt("COUNT_ADS", count);
		}
		PlayerPrefs.Save();
		#else
		if(count >= numberOfPlayToShowInterstitial)
		{
			Debug.LogWarning("To show ads, please have a look to Very Simple Ad on the Asset Store, or go to this link: " + verySimpleAdLink);
			Debug.LogWarning("Very Simple Ad is already implemented in this asset");
			Debug.LogWarning("Just import the package and you are ready to use it and monetize your game!");
			Debug.LogWarning("Very Simple Ad : " + verySimpleAdLink);
			PlayerPrefs.SetInt("GAMEOVER_COUNT",0);
		}
		else
		{
			PlayerPrefs.SetInt("GAMEOVER_COUNT", count);
		}
		PlayerPrefs.Save();
		#endif

	}


//	public Color GetColor(DColor c)
//	{
//		if(c == DColor.RED)
//		{
//			return red;
//		}
//		else if(c == DColor.GREEN)
//		{
//			return green;
//		}
//		else if(c == DColor.BLUE)
//		{
//			return blue;
//		}
//		else if(c == DColor.YELLOW)
//		{
//			return yellow;
//		}
//	}
}
