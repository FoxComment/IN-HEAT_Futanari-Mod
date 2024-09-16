using MelonLoader;
using UnityEngine;
using Il2CppMonsterBox;
using Il2CppSteamworks;
using UnityEngine.Rendering.Universal;
using Il2CppParadoxNotion;
using Harmony;
using static Mono.Security.X509.X520;
using System.Reflection.Metadata;
using System.Reflection;
using System.Collections;
using UnityEngine.XR;
using Il2CppMono.Unity;
using Il2CppNodeCanvas.Tasks.Actions;
using Il2CppSystem;
using UnityEngine.UI;
using Il2CppInterop.Runtime;
using Il2CppTMPro;
using UnityEngine.TextCore.Text;
using I18N.Common;
using Mono.WebBrowser;
using FutaMod;
[assembly: MelonInfo(typeof(ModMain), "FutanariMod", "10.10.2024", "FoxComment", "https://github.com/foxcomment/inheat_futamod")]
[assembly: MelonGame("MonsterBox", "IN HEAT")]
namespace FutaMod
{

    public class ModMain : MelonMod
    {



        float timeTest;
        float fov;
        Camera myCam = null;
        Transform muhObjectTRA = null;
        GameObject[] goNames = new GameObject[0];
        int itmID = 0;
        string objectList = " asdvdfvbsed df s d sdfg sedfdfgedfsdffvgds vbdf ";
        Vector2 scrollPosition= Vector2.zero;
        //string[] listDirectoryActive = new string[0];
        GameObject[] listObjectDirectoryActive = new GameObject[0];
        GameObject focusedObject;
        string objectsk="";
        private const string SubModFile = "Assets/Futa/Mesh/FoAva.fbx";
        private const string SubTeFile = "Assets/Futa/Teqstures/DiccensMine.png";
        private const string SubMatFile = "Assets/mofolder/SubMat.mat";
        Il2CppAssetBundle bundle;

        GameObject testObj;

        public GameObject myGO;

        TMP_FontAsset myFont;
        Mesh myMesh;

        
        /// <summary>
        /// LegacyRuntime.ttf
        /// </summary>




        GameObject mainEplorer;
        GameObject draggableWindow;
        GameObject inputFieldThing;
        GameObject teqstInput;
        Material myMaterial;
        Mesh myTeqsture;
        int tmpStringAddressID = 0;

        public string addressField= "InHeatFutaMod.funny.assetbundle";
        public override void OnInitializeMelon()
        {/*
            MemoryStream memoryStream;

            using (Stream stream = assembly.GetManifestResourceStream(SubModFile))
            {
                memoryStream = new MemoryStream((int)stream.Length);
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                memoryStream.Write(buffer, 0, buffer.Length);
            }

            bundle = AssetBundle.LoadFromMemory(memoryStream.ToArray());*/


//            done = true;
        }



        public override void OnSceneWasInitialized(int buildindex, string sceneName)
        {
            myFont = GameObject.FindObjectsOfType<TMP_Text>()[0].font;



            System.Console.WriteLine("Font ("+ myFont.name+")");


            mainEplorer = new GameObject();
            mainEplorer.name = "TestCanvas";
            mainEplorer.AddComponent<Canvas>();
            mainEplorer.AddComponent<CanvasRenderer>();
            mainEplorer.AddComponent<CanvasScaler>();
            mainEplorer.AddComponent<GraphicRaycaster>();
            mainEplorer.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

            draggableWindow = new GameObject();
            draggableWindow.name = "img";
            draggableWindow.transform.parent = mainEplorer.transform;
            draggableWindow.AddComponent<RectTransform>();
            draggableWindow.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 300);
            draggableWindow.GetComponent<RectTransform>().localPosition = new Vector2(50, 10);
            draggableWindow.AddComponent<Image>();
            draggableWindow.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
            draggableWindow.AddComponent<VerticalLayoutGroup>();
            draggableWindow.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(5, 5, 5, 5);


            System.Console.WriteLine("InputField Creating");
            inputFieldThing = new GameObject();
            inputFieldThing.name = "infiel";
            inputFieldThing.transform.parent = draggableWindow.transform;
            inputFieldThing.AddComponent<TMP_InputField>();
            inputFieldThing.AddComponent<Image>();

            System.Console.WriteLine("Input Field Created.");
            teqstInput = new GameObject();
            teqstInput.name = "TeIn";
            inputFieldThing.GetComponent<Image>().color = new Color(1, 1, 1, .5f);


            teqstInput.transform.parent = inputFieldThing.transform;



            System.Console.WriteLine("Canvas Done");



            teqstInput.AddComponent<TextMeshProUGUI>();

            System.Console.WriteLine("TM Created: ,is null? "+ teqstInput.GetComponent<TextMeshProUGUI>() == null+ "  "+teqstInput.GetComponent<TMP_Text>());
            System.Console.WriteLine("Font: "+ myFont);
            System.Console.WriteLine("Font2: " + teqstInput.GetComponent<TextMeshProUGUI>());


            teqstInput.GetComponent<TextMeshProUGUI>().font = myFont;
            System.Console.WriteLine("Font Assigned");
            teqstInput.GetComponent<TextMeshProUGUI>().material = myFont.material;
            System.Console.WriteLine("Mat Assigned");
            teqstInput.GetComponent<TextMeshProUGUI>().color = Color.red;
            teqstInput.GetComponent<TextMeshProUGUI>().text = "sdsdsd";
            inputFieldThing.GetComponent<TMP_InputField>().textComponent = teqstInput.GetComponent<TextMeshProUGUI>();
            


            
            //MelonDebug.Msg("Debug TESSTSTT");

            //UnityEngine.Il2CppAssetBundle bundle = Il2CppAssetBundleManager.LoadFromFile("MoFolder/SubC.fbx");
            //MelonDebug.Msg("Loaded Bundle (?)");
            //MelonDebug.(" - asset");
            //MelonDebug.Msg(" - asset2");
            //GameObject sluber = bundle.LoadAsset<GameObject>("SubC");
            //testObj = GameObject.Instantiate(sluber);
            //testObj.transform.position = Camera.main.transform.position;
            //testObj.SetActive(true);


            /*
            objectList = "<size=30>NEW LIST</size> \n";
            goNames = GameObject.FindObjectsOfType<GameObject>();
            //listObjectDirectoryActive = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            /*
            foreach (GameObject _go in goNames)
            {
                itmID++;
                objectList += _go.name + " , ";
                listObjectDirectoryActive = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            }
            */
            // FetchObjects(GameObject.Find("Tape").transform.parent.gameObject);
            /*



            
            foreach (GameObject _go in goNames)
            {
                itmID++;
                objectList+= _go.name +" , ";

                if (itmID > 10)
                {
                    itmID = 0;
                    objectList += "\n";
                }
            }*/
        }

        IEnumerator Waiter()
        {
            yield return new WaitForSeconds(1);

            inputFieldThing.GetComponent<TMP_InputField>().textComponent = teqstInput.GetComponent<TMP_Text>();
        }

        void LoadAssetBundle()
        {
            System.Console.WriteLine("Load Asset from (" + addressField +") ...");

           /* Il2CppAssetBundle bundle = Il2CppAssetBundleManager.LoadFromFile(addressField);
            System.Console.WriteLine("Load Passed...");
            System.Console.WriteLine("Loaded ( "+bundle+" )");
            System.Console.WriteLine("Loaded Asets( "+bundle.AllAssetNames()+ " )");

            GameObject sluber = bundle.LoadAsset<GameObject>("SubC");
            testObj = GameObject.Instantiate(sluber);
            testObj.transform.position = Camera.main.transform.position;
            testObj.SetActive(true);
           */


            MemoryStream memoryStream;

            using (Stream stream = MelonAssembly.Assembly.GetManifestResourceStream(addressField))
            {
                System.Console.WriteLine("Stream Started");
                memoryStream = new MemoryStream((int)stream.Length);
                byte[] buffer = new byte[stream.Length];
                System.Console.WriteLine("Read");
                stream.Read(buffer, 0, buffer.Length);
                System.Console.WriteLine("Write");
                memoryStream.Write(buffer, 0, buffer.Length);
            }

            System.Console.WriteLine("Assign");
            AssetBundle bundle = AssetBundle.LoadFromMemory(memoryStream.ToArray());


            System.Console.WriteLine("Done");
            System.Console.WriteLine(bundle.name +" -name");
            System.Console.WriteLine("\n\n Listing:");

            string[] objNames = bundle.GetAllAssetNames();
            foreach (string objName in objNames) 
            System.Console.WriteLine(objName);


            myMesh = bundle.LoadAsset_Internal(SubModFile, Il2CppType.Of<Mesh>()).Cast<Mesh>();
            System.Console.WriteLine("Got Mesh");
            System.Console.WriteLine(myMesh.name);

            myMesh.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            System.Console.WriteLine("Got Mesh");

           // myMaterial = bundle.LoadAsset_Internal(SubMatFile, Il2CppType.Of<Material>()).Cast<Material>();
            //myMaterial.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            //System.Console.WriteLine(myMaterial.name);
            System.Console.WriteLine("FUQ, YEAH.");


            //   manager

            /*
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("c"))
            using (var tempStream = new MemoryStream((int)stream.Length))
            {
                stream.CopyTo(tempStream);

                myAssetBundle = AssetBundle.LoadFromMemory_Internal(tempStream.ToArray(), 0);
                myAssetBundle.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            }

            myJoinSprite = myAssetBundle.LoadAsset_Internal("Assets/mofolder/SubCTeqs.png", Il2CppType.Of<Sprite>()).Cast<Sprite>();
            myJoinSprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            */

            // myJoinClip = myAssetBundle.LoadAsset_Internal("Assets/JoinNotifier/Chime.ogg", Il2CppType.Of<AudioClip>()).Cast<AudioClip>();


        }



        void FetchObjects(GameObject _objID)
        {
            objectList = "<size=30>NEW LIST</size> \n";
            goNames = GameObject.FindObjectsOfType<GameObject>();

            listObjectDirectoryActive = new GameObject[_objID.transform.childCount-1];
            for (int i = 0; i < _objID.transform.childCount - 1; i++)
            {
                itmID++;
                listObjectDirectoryActive[i] = _objID.transform.GetChild(i).gameObject;

                if (itmID > 10)
                {
                    itmID = 0;
                    objectList += "\n";
                }
            }
        }


        public override void OnGUI()
        {
            //scrollPosition = GUI.BeginScrollView(new Rect(10, 10, 1900, 1000), scrollPosition, new Rect(0, 0, 1900, 3000));

            for (int i = 0; i < listObjectDirectoryActive.Length-1; i++)
            {
          //      if (GUI.Button(new Rect(10+(i*30), 10, 500, 30), listObjectDirectoryActive[i].name))
            //        FetchObjects(listObjectDirectoryActive[i]);

            }

            //GUI.Box(new Rect(5, 5, 1800, 3000), objectList);

            //addressField = GUI.TextField(new Rect(10, 10, 200, 20), addressField, 25);
            //GUI.EndScrollView();
        }




        public override void OnLateUpdate()
        {



            ///MUNCHING





            if ((Input.GetKeyDown(KeyCode.H)))
            {
             //   addressField = possibleAddresses[tmpStringAddressID];
               // tmpStringAddressID++;
                //if(tmpStringAddressID > 4) tmpStringAddressID = 0;
                LoadAssetBundle();
            }


            
            if ((Input.GetKeyDown(KeyCode.C ))){

                // Camera.main.enabled = false;

                muhObjectTRA = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
                muhObjectTRA.localScale = Vector3.one*100f;
                System.Console.WriteLine("Mesh Spawned");
                MelonDebug.Msg("DDDDD");
                muhObjectTRA.gameObject.GetComponent<MeshFilter>().mesh = myMesh;
                System.Console.WriteLine("Assigned");
                muhObjectTRA.position = Camera.main.transform.position;
                muhObjectTRA.rotation = Quaternion.identity;

                System.Console.WriteLine("Assigned Pos");
                muhObjectTRA.gameObject.AddComponent<Light>();
                muhObjectTRA.Rotate(Vector3.left * 90);
                muhObjectTRA.gameObject.GetComponent<Light>().range = 1250;
                //muhObjectTRA.tag = Camera.main.tag;

                myMaterial = GameObject.FindObjectsOfType<MeshRenderer>()[0].material;
                muhObjectTRA.GetComponent<MeshRenderer>().material = myMaterial;


                //myCam = muhObjectTRA.gameObject.AddComponent<Camera>();
                //myCam.targetDisplay = 1;
                //myCam.stereoTargetEye = StereoTargetEyeMask.Both;
                muhObjectTRA.gameObject.GetComponent<BoxCollider>().enabled = false;

            }
        }
    }
}
