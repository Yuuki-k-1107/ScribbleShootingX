using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class drawCanvas : MonoBehaviour
{
	public List<Vector2> tList = new List<Vector2>();
    public Texture2D drawTexture;
	public Color[] buffer;
	public Vector2 prePosition;
	public Vector2 position;
	public Vector2 startPosition;
	Vector2 drawPositionx;
	Vector2 drawPositiony;
	Vector2 direction;
	// 線の太さ
	[SerializeField] int lineThickness = 10;
	float countLength = 0f;
	// 軌道に含められる点の数
	[SerializeField] float maxLength = 6000.0f;
	public float lineCost;
	char saveNumber;
	GameObject manager;
	saveData data;
    // Start is called before the first frame update
    void Start()
    {
		Application.targetFrameRate = 240;
		manager = GameObject.Find ("manager");
		data = manager.GetComponent<dataManager>().data; 
        Texture2D mainTexture = (Texture2D) GetComponent<Renderer>().material.mainTexture;
		Color[] pixels = mainTexture.GetPixels();

		buffer = new Color[pixels.Length];
		pixels.CopyTo (buffer, 0);

		drawTexture = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);
		drawTexture.filterMode = FilterMode.Point;
    }

	public void Draw(Vector2 p, Color c)
	{
		int startX = Mathf.Max(0, (int) p.x - lineThickness);
    	int startY = Mathf.Max(0, (int) p.y - lineThickness);
    	int endX = Mathf.Min(drawTexture.width, (int) p.x + lineThickness + 1);
    	int endY = Mathf.Min(drawTexture.height, (int) p.y + lineThickness + 1);
		for(int i = startX; i < endX; i++){
			for(int j = startY; j < endY; j++){
				int index = i + drawTexture.width * j;
				buffer.SetValue(c, index);
			}
		}
	}

	public void Clear()
	{
		Texture2D mainTexture = (Texture2D) GetComponent<Renderer>().material.mainTexture;
		Color[] pixels = mainTexture.GetPixels();
		for(int i = 0; i < pixels.Length; i++){
			buffer.SetValue(Color.white, i);
		}
	}


	public Color lineColor(Vector2 p){
		Vector2 cp = p - prePosition;
		float cr = (countLength + cp.magnitude) / maxLength;
		
		return Color.HSVToRGB(H: 0.75f * (1.0f - cr), 1.0f, 1.0f);
	}

	public void addLength(){	
		if(tList.Count != 0) lineCost = countLength;
		countLength += direction.magnitude;
		tList.Add(direction);
	}

	public void drawLine(){
		direction = position - prePosition;
		Vector2 sDirection = new Vector2(0, 0);
		if(direction.x > 0){
			sDirection.x = 1.0f;
		}
		else if(direction.x < 0){
			sDirection.x = -1.0f;
		}
		if(direction.y > 0){
			sDirection.y = 1.0f;
		}
		else if(direction.y < 0){
			sDirection.y = -1.0f;
		}
		drawPositionx = prePosition;
		drawPositiony = prePosition;

		while(((int) drawPositionx.x != (int) position.x && (int) drawPositiony.x != (int) position.x) || ((int) drawPositionx.y != (int) position.y && (int) drawPositiony.y != (int) position.y)){						
			if((drawPositionx.y + sDirection.x * direction.y / direction.x) * sDirection.y <= (drawPositiony.y + sDirection.y) * sDirection.y){
				drawPositionx.x += sDirection.x;
				drawPositionx.y += sDirection.x * direction.y / direction.x;
				if(drawPositionx.y * sDirection.y > position.y * sDirection.y) break;
				Draw(drawPositionx, lineColor(drawPositionx));
			}
			else{
				drawPositiony.y += sDirection.y;
				drawPositiony.x += sDirection.y * direction.x / direction.y;
				if(drawPositiony.x * sDirection.x > position.x * sDirection.x) break;
				Draw(drawPositiony, lineColor(drawPositiony));
			}
		}

		Draw(position, lineColor(position));
	}

	public void loadData(List<Vector2> t, float c, Vector2 s){
		Clear();
		drawTexture.SetPixels(buffer);
		drawTexture.Apply();
		GetComponent<Renderer>().material.mainTexture = drawTexture;
		lineCost = c;
		startPosition = s;
		tList = new List<Vector2>(t);
		countLength = 0f;
		Draw(startPosition, lineColor(startPosition));

		prePosition = startPosition;
		countLength++;
		for(int i = 0; i < tList.Count; i++){
			position = prePosition+tList[i];
			drawLine();
			prePosition = position;
			countLength += tList[i].magnitude;
		}
		drawTexture.SetPixels(buffer);
		drawTexture.Apply();
		GetComponent<Renderer>().material.mainTexture = drawTexture;
	}

    // Update is called once per frame
    void Update()
    {
		if(!canvasFlag.isSaveOrLoad){
			if(Input.GetMouseButton(0)){
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if(Physics.Raycast(ray, out hit, 100.0f)) {
					position = hit.textureCoord * new Vector2(drawTexture.width, drawTexture.height);
					if(countLength == 0f){
						prePosition = position;
						startPosition = position;
						Draw(position, lineColor(position));
						countLength++;
					}
					else if(countLength < maxLength && !position.Equals(prePosition)){
						drawLine();
						addLength();
						prePosition = position;
					}
				}
				drawTexture.SetPixels (buffer);
				drawTexture.Apply ();
				GetComponent<Renderer> ().material.mainTexture = drawTexture;
			}
			else if(Input.GetMouseButton(1)){
				Clear();
				countLength = 0f;
				drawTexture.SetPixels (buffer);
				drawTexture.Apply ();
				GetComponent<Renderer> ().material.mainTexture = drawTexture;
				tList = new List<Vector2>();
				lineCost = 0f;
			}
		}
	}
}
