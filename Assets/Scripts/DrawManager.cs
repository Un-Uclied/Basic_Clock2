using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DrawManager : Singleton<DrawManager>
{
    [SerializeField] private GameObject linePrefab;

    private void Start(){
        SceneManager.activeSceneChanged += (Scene o, Scene n) =>{
            if (n.name == "MainClock"){
                LoadDrawings();
            }
            else if (n.name == "Settings"){
                GlobalHandler.Instance.lineObjects.Clear();
            }
            
        };
    }

    public void EraseDrawings(){
        foreach(var o in GlobalHandler.Instance.lineObjects){
            Destroy(o);
        }
        GlobalHandler.Instance.lineObjects.Clear();
        GlobalHandler.Instance.lineStrokes.Clear();
    }

    public void LoadDrawings(){
        if (!GlobalHandler.Instance.isDrawMode){
            EraseDrawings();
            return;
        }
        for(int i = 0; i < GlobalHandler.Instance.lineStrokes.Count; i++){
            GameObject stroke = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
            stroke.transform.SetParent(FindAnyObjectByType<Canvas>().transform);
            LineRendererUI lr = stroke.GetComponent<LineRendererUI>();
            lr.thickness = GlobalHandler.Instance.currentWidth;
            stroke.name = i.ToString();

            int pointCnt = 0;
            for(int j = 0; j < GlobalHandler.Instance.lineStrokes[i].Length; j++){
                Array.Resize(ref lr.points, ++pointCnt);
                lr.points[pointCnt - 1] = GlobalHandler.Instance.lineStrokes[i][j];
                //lr.SetPosition(j, GlobalHandler.Instance.lineStrokes[i][j]);
            }
            lr.UpdateGeometry();
            GlobalHandler.Instance.lineObjects.Add(stroke.gameObject);
        }
    }

}