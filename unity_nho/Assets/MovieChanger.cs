using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using DG.Tweening;

public class MovieChanger : MonoBehaviour {

	//能のvideo
	public VideoPlayer video0;
	public VideoPlayer video1;
	public VideoPlayer video2;
	//videoの配列無理やり突っ込む
	public VideoPlayer [] video;
	//fadeout用の黒いパネル
	public CanvasGroup fadeout;
	//初めに文字出すためのパネル
	public CanvasGroup textCanvas;
	//現在再生されてるvideoの番号(0が1番目のvideo)
	public int nowPlayVideo;



	// Use this for initialization
	void Start () {
		Init();
	}
	
	// Update is called once per frame
	void Update () {
		//→と←で動画の操作可能
		if(Input.GetKeyDown(KeyCode.RightArrow)){
			if (nowPlayVideo != video.Length-1) {
				video [nowPlayVideo].loopPointReached -= Loop;
				nowPlayVideo++;
				video [nowPlayVideo].loopPointReached += Loop;
				StartCoroutine ("blackOut");
			}
		}else if(Input.GetKeyDown(KeyCode.LeftArrow)){
			if(nowPlayVideo != 0){
				video [nowPlayVideo].loopPointReached -= Loop;
				nowPlayVideo--;
				video [nowPlayVideo].loopPointReached += Loop;
				StartCoroutine ("blackOut");
			}
		}
	}

	void Init(){
		//配列の初期化
		video = new VideoPlayer[3];

		//videoは全て配列にぶち込む
		video [0] = video0;
		video [1] = video1;
		video [2] = video2;

		//videoを止めた状態でスタート
		for(int i=0;i <= video.Length-1;i++){
			video [i].Stop ();
			video [i].isLooping = true;
		}

		//presented by fukuchilabのフェードアウトのコルーチン
		StartCoroutine ("StartText");

		//videoは0番目から始まる
		nowPlayVideo = 0;
		video [0].loopPointReached += Loop;
	}

	//特定のvideoだけ再生して他を止めておく関数
	void videoStopAndStart(int videoNum){
		for(int i=0;i <= video.Length-1;i++){
			video [i].Stop ();
		}
		video [videoNum].Play ();
	}
		

	//動画が終了した瞬間Loopに入るときに呼び出される、かつ自動で次のvideoにうつる関数
	void Loop(VideoPlayer vp){
		if(nowPlayVideo != video.Length-1){
			video[nowPlayVideo].Stop();
			video [nowPlayVideo].loopPointReached -= Loop;
			nowPlayVideo++;
			video [nowPlayVideo].loopPointReached += Loop;
			video [nowPlayVideo].Play ();
		}
	}

	//スタートのコルーチン
	private IEnumerator StartText(){
		yield return new WaitForSeconds (2f);
		DOTween.To (() => textCanvas.alpha, (x) => textCanvas.alpha = x, 0.0f, 3.0f).SetEase(Ease.InCubic);
		DOTween.To (() => fadeout.alpha , (x) =>fadeout.alpha = x ,1.0f,1.0f).SetEase(Ease.InCubic);
		yield return new WaitForSeconds (3f);
		video [0].Play ();
		DOTween.To (() => fadeout.alpha , (x) =>fadeout.alpha = x ,0.0f,1.0f).SetEase(Ease.InCubic);
	}

	//ブラックアウトのコルーチン
	private IEnumerator blackOut(){
		DOTween.To (() => fadeout.alpha , (x) =>fadeout.alpha = x ,1.0f,1.0f).SetEase(Ease.InCubic);
		yield return new WaitForSeconds (1.0f);
		videoStopAndStart (nowPlayVideo);
		DOTween.To (() => fadeout.alpha , (x) =>fadeout.alpha = x ,0.0f,1.0f).SetEase(Ease.InCubic);
	}
}
