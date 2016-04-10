using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuScript : MonoBehaviour {

	public void ReloadGame() {

		SceneManager.LoadScene (0, LoadSceneMode.Single);
	}

	public void TakeUserStudy() {

		string uri = "https://docs.google.com/forms/d/1Xu7Efs2JVjsfC7zr1rNdQEnWgvcBa9mSpu3-vrTCPlI/edit?usp=sharing_eid&ts=56f5bab0";
		//System.Net.WebClient client = new System.Net.WebClient ();
		//client.DownloadData (new System.Uri("https://docs.google.com/forms/d/1Xu7Efs2JVjsfC7zr1rNdQEnWgvcBa9mSpu3-vrTCPlI/edit?usp=sharing_eid&ts=56f5bab0"));
		/*System.Diagnostics.Process process = new System.Diagnostics.Process();
		//process.StartInfo.FileName = uri;
		process.StartInfo.FileName = "firefox.exe";
		process.StartInfo.Arguments = "\"" + uri + "\"";
		process.Start ();
		*/
		Application.OpenURL (uri);
	}
}
