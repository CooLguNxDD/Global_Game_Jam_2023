using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{

	public Sprite[] sprites;
	public int framesPerSprite;
	public bool loop;
    private float delayStart;

	private int index = 0;
	private Image image;
	private int frame = 0;
	private float timer = 0;

	void Awake()
	{
		image = GetComponent<Image>();
	}

    private void Start()
    {
		delayStart = Random.Range(0.0f, 5.0f);
	}

    void Update()
	{
		timer += Time.deltaTime;
		if (delayStart > timer) return;
		if (!loop && index == sprites.Length) return;
		frame++;
		if (frame < framesPerSprite) return;
		image.sprite = sprites[index];
		frame = 0;
		index++;
		if (index >= sprites.Length)
		{
			if (loop) index = 0;
		}
	}
}