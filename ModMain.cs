using MelonLoader;
using UnityEngine;
using Il2CppMonsterBox;
using Il2CppSteamworks;
using UnityEngine.Rendering.Universal;
using Il2CppParadoxNotion;
using Harmony;
using HarmonyLib;
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
using Il2Cpp;
using Il2CppMonsterBox.Runtime.Core.Gameplay;
using UnityEngine.Playables;
using Il2CppMonsterBox.Systems.Tools.Miscellaneous;
using Il2CppMonsterBox.Systems;
using Il2CppMonsterBox.Systems.Tools;
using Il2CppMonsterBox.Systems.Tools.Accessors;
using Il2CppMonsterBox.Systems.Tools.Interfaces;
using Il2CppMonsterBox.Systems.Tools.PubSub;
using Il2CppMonsterBox.Systems.Tools.PubSub.Interfaces;
using Il2CppMonsterBox.Systems.Camera;
using Il2CppMonsterBox.Systems.Input;
using Il2CppMonsterBox.Systems.Subtitle;
using Il2CppMonsterBox.Systems.Subtitle.Scriptables;
using Il2CppMonsterBox.Systems.SharpConfig;
using Il2CppMonsterBox.Systems.Saving.Enums;
using Il2CppMonsterBox.Systems.Saving.Scriptables;
using Il2CppMonsterBox.Systems.Saving.Data;
using Il2CppMonsterBox.Systems.Confirmation.Adult;
using Il2CppMonsterBox.Systems.Dialogue.UI.Keyboard;
using Il2CppMonsterBox.Systems.Dialogue.UI;
using Il2CppMonsterBox.Runtime.Core.Initializers;
using Il2CppMonsterBox.Runtime.Core.SceneManagement;
using Il2CppMonsterBox.Runtime.Core.Scriptable;
using Il2CppMonsterBox.Runtime.Extensions._Localization;
using Il2CppMonsterBox.Extensions.Addressables;
using Il2CppMonsterBox.Extensions.Rewired.Button_Hint.UI;
using Il2CppMonsterBox.Extensions.Rewired.Button_Hint;
using Il2CppMonsterBox.Runtime.Gameplay.Nightclub.Character;
using Il2CppMonsterBox.Runtime.Gameplay;
using Mono.CSharp;
using Il2CppDG.Tweening.Core;
using Il2CppMonsterBox.Systems.Tools.Miscellaneous.Adult;
using Il2CppMonsterBox.Runtime.Gameplay.Nightclub.Enums;
using Il2CppMonsterBox.Runtime.UI.Settings;
using Il2CppMonsterBox.Runtime.Gameplay.Nightclub.Level;
using Il2CppMonsterBox.Runtime.Gameplay.Enums;
using Il2CppMonsterBox.Runtime.Gameplay.SecurityOffice;
using Il2CppMonsterBox.Runtime.Gameplay.Character;
using Il2CppRewired.Platforms;
using static UnityEngine.UIElements.UIRAtlasAllocator;
using static UnityEngine.Rendering.Universal.UniversalRenderPipeline.Profiling;
using System.ComponentModel;
[assembly: MelonInfo(typeof(ModMain), "FutanariMod", "10.10.2024", "FoxComment", "https://github.com/foxcomment/inheat_futamod")]
[assembly: MelonGame("MonsterBox", "IN HEAT")]
namespace FutaMod
{

    public class ModMain : MelonMod
    {
        Transform cameraTRA = null;
        GameObject[] goNames = new GameObject[0];
        string objectList = " asdvdfvbsed df s d sdfg sedfdfgedfsdffvgds vbdf ";
        GameObject[] listObjectDirectoryActive = new GameObject[0];

        public string addressField = "InHeatFutaMod.funny.assetbundle";

        private const string textureAppendageSammyFile = "Assets/Futa/Textures/SammyV1.png";
        private const string textureAppendageMaddyFile = "Assets/Futa/Textures/MaddyV1.png";
        private const string textureAppendagePoppiFile = "Assets/Futa/Textures/PoppiV1.png";
        private const string textureAppendageAriFile = "Assets/Futa/Textures/AriV1.png";
        private const string textureAppendageNileFile = "Assets/Futa/Textures/NileV1.png";

        private const string spriteHContentFile = "Assets/Futa/Textures/IntersexButtonBG.png";

        private const string appendageOnePrefabFile = "Assets/Futa/Prefs/AppengadeOne.prefab";

        Texture2D textureAppendageSammy;
        Texture2D textureAppendageMaddy;
        Texture2D textureAppendagePoppi;
        Texture2D textureAppendageAri;
        Texture2D textureAppendageNile;

        Sprite spriteHContent;

        CharacterControllerBase[] currentCharacters;

        AdultSwitcher adultSwitch;

        GameObject prefabAppendageOne;

        Material defaultMaterial;
        Shader defaultShader;

        List<GameObject> activeAppendages = new List<GameObject>(0);

        public override void OnInitializeMelon()
        {
            LoadAssetBundle();            
        }




        IEnumerator FetchCharacters()
        {
            //Give this thing a moment 2 think cus i have no idea on how to access delegates in il2cpp :p
            yield return new WaitForSeconds(.5f);

            currentCharacters = GameObject.FindObjectsOfType<CharacterControllerBase>();
            DBG_ListActiveCharacteds();

            yield return new WaitForSeconds(.1f);
            SpawnAppendages();
        }


        void DBG_ListActiveCharacteds()
        {
            MelonLogger.Msg("Girls : " + currentCharacters.Length);

            foreach (CharacterControllerBase item in currentCharacters)
                MelonLogger.Msg("   * " + item.gameObject.name);
        }



        void SpawnAppendages()
        {
            MelonLogger.Msg("Shader: "+ defaultShader.name);

            foreach (CharacterControllerBase _character in currentCharacters)
            {
                var children = _character.Animator.transform.GetChild(0).GetComponentsInChildren<Transform>();
                foreach (var child in children)
                    if (child.name == "DEF-spine")
                    {
                        ApplyAppendage(_character, child.transform);
                        break;
                    }
            }
        }


        IEnumerator AppendageAlive(SkinnedMeshRenderer _appendageSkinned, CharacterControllerBase _controller)
        {

            _controller.CostumeSwitcher.defaultVariant = "Nude";

            _controller.CostumeSwitcher.SwitchVariant("Nude");


            Animator _anim = _appendageSkinned.transform.parent.GetComponent<Animator>();
            MelonLogger.Msg("Cuck Created for " + _controller.character + " Deciding Stuff");
            switch (Il2CppMonsterBox.Runtime.Gameplay.Level.LevelManagerBase.Level)
            {
                case Level.SecurityOffice:
                    SecurityOfficeCharacterController _secController = _controller.GetComponent<SecurityOfficeCharacterController>(); 
                    while (_appendageSkinned)
                    {
                        _anim.SetFloat("Enlarge", .5f);

                        yield return new WaitForSeconds(.5f);
                    }
                    break;

                case Level.Nightclub:
                    NightclubCharacterController _nigController = _controller.GetComponent<NightclubCharacterController>();
                    while (_appendageSkinned)
                    {
                        _anim.SetFloat("Enlarge", _nigController._excitement._excitement);
                        _anim.SetBool("Leak", _nigController.IsExcited);

                        yield return new WaitForSeconds(.2f);
                    }
                    break;
            }

        }

        private void ApplyAppendage(CharacterControllerBase _character, Transform _hipBone)
        {
            MelonLogger.Msg("App Added for: " + _hipBone.name);

            GameObject _appendage = UnityEngine.Object.Instantiate(prefabAppendageOne, _hipBone);
            SkinnedMeshRenderer _skinnedAppendage = _appendage.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
            Animator _anim = _appendage.GetComponent<Animator>();

            _skinnedAppendage.material = defaultMaterial;
            _skinnedAppendage.material.shader = defaultShader;

            MelonLogger.Msg("Shader Assigned");

            _appendage.transform.localRotation = Quaternion.Euler(0, 0, 0);
            _skinnedAppendage.material.SetFloat("_Smoothness", 0);
            _skinnedAppendage.material.SetFloat("_EnvironmentReflections", 0);

            MelonLogger.Msg("Material Configured");

            switch (_character.character) 
            {
                case Characters.Ari:
                    _skinnedAppendage.material.mainTexture = textureAppendageAri;
                    _appendage.transform.localScale = Vector3.one * .49f;
                    _appendage.transform.localPosition = Vector3.forward * .103f;
                    break;

                case Characters.Sammy:
                    _skinnedAppendage.material.mainTexture = textureAppendageSammy;
                    _appendage.transform.localScale = Vector3.one * .68f;
                    _appendage.transform.localPosition = Vector3.forward * .068f;
                    _anim.SetFloat("Fertility", .2f);
                    _anim.SetFloat("Thickness", .3f);
                    break;

                case Characters.Poppi:
                    _skinnedAppendage.material.mainTexture = textureAppendagePoppi;
                    _appendage.transform.localScale = Vector3.one * .66f;
                    _appendage.transform.localPosition = Vector3.forward * .071f;
                    _anim.SetFloat("Fertility", .1f);
                    break;

                case Characters.Nile:
                    _skinnedAppendage.material.mainTexture = textureAppendageNile;
                    _appendage.transform.localScale = Vector3.one * .55f;
                    _appendage.transform.localPosition = Vector3.forward * .092f;
                    _anim.SetFloat("Fertility", .15f);
                    break;

                case Characters.Maddie:
                    _skinnedAppendage.material.mainTexture = textureAppendageMaddy;
                    _appendage.transform.localPosition = Vector3.forward * .13f;
                    _appendage.transform.localScale = Vector3.one * .7f;
                    _anim.SetFloat("Fertility", .35f);
                    _anim.SetFloat("Thickness", .7f);
                    break;
                
                case Characters.Misty:
                    _skinnedAppendage.material.mainTexture = textureAppendageMaddy;
                    _appendage.transform.localPosition = Vector3.forward * .2f;
                    _appendage.transform.localScale = Vector3.one * .4f;
                    _anim.SetFloat("Fertility", 0f);
                    _anim.SetFloat("Thickness", 1f);
                    break;

                case Characters.Kass:
                    UnityEngine.Object.Destroy(_appendage);
                    return;
            }

            MelonCoroutines.Start(AppendageAlive(_skinnedAppendage, _character));

            MelonLogger.Msg(_character +"  Is dicked");

            activeAppendages.Add(_appendage);
        }



        public override void OnSceneWasInitialized(int buildindex, string sceneName)
        {
            currentCharacters = new CharacterControllerBase[0];



            if (defaultShader == null)
                if (GameObject.FindObjectOfType<SkinnedMeshRenderer>() != null)
                {
                    defaultShader = GameObject.FindObjectOfType<SkinnedMeshRenderer>().material.shader;
                    defaultShader.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                    MelonLogger.Msg(defaultShader.name);
                }

            if (Il2CppMonsterBox.Runtime.Gameplay.Level.LevelManagerBase.Exists)
                switch (Il2CppMonsterBox.Runtime.Gameplay.Level.LevelManagerBase.Level)
                {
                    case Level.SecurityOffice:
                        MelonLogger.Msg("Office Level");
                        MelonCoroutines.Start(FetchCharacters());
                        break;
                    case Level.Nightclub:
                        MelonLogger.Msg("NightClub Level");
                        MelonCoroutines.Start(FetchCharacters());
                        break;
                }else
            if (GameObject.FindObjectOfType<AdultSpriteSwitcher>() != null)
            {
                AdultSpriteSwitcher _ad = GameObject.FindObjectOfType<AdultSpriteSwitcher>();
                SettingsAdultSection dd;
                _ad._nsfwSprite = spriteHContent;
                _ad.UpdateSprite(_ad.transform.GetChild(0).GetComponent<Image>().sprite.texture.name != "Sprite_DetectiveRoom_ScreenButton_Gallery");
            }

        }


        void LoadAssetBundle()
        {
            System.Console.WriteLine("Load Asset from (" + addressField + ") ...");

            MemoryStream memoryStream;

            using (Stream stream = MelonAssembly.Assembly.GetManifestResourceStream(addressField))
            {
                memoryStream = new MemoryStream((int)stream.Length);
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                memoryStream.Write(buffer, 0, buffer.Length);
            }

            AssetBundle bundle = AssetBundle.LoadFromMemory(memoryStream.ToArray());

            System.Console.WriteLine(bundle.name + " -name");
            MelonLogger.Msg("\n\n Listing:");

            string[] objNames = bundle.GetAllAssetNames();
            foreach (string objName in objNames)
                MelonLogger.Msg("   * " + objName);

            prefabAppendageOne = bundle.LoadAsset_Internal(appendageOnePrefabFile, Il2CppType.Of<GameObject>()).Cast<GameObject>();

            textureAppendagePoppi = bundle.LoadAsset_Internal(textureAppendagePoppiFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            textureAppendageNile = bundle.LoadAsset_Internal(textureAppendageNileFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            textureAppendageSammy = bundle.LoadAsset_Internal(textureAppendageSammyFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            textureAppendageAri = bundle.LoadAsset_Internal(textureAppendageAriFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            textureAppendageMaddy = bundle.LoadAsset_Internal(textureAppendageMaddyFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();

            Texture2D _tmpBNSF = bundle.LoadAsset_Internal(spriteHContentFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            spriteHContent = Sprite.Create(_tmpBNSF, new Rect(0, 0, _tmpBNSF.width, _tmpBNSF.height), Vector2.one * .5f, 100);

            System.Console.WriteLine("Got Mesh");

            prefabAppendageOne.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            textureAppendagePoppi.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            textureAppendageNile.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            textureAppendageSammy.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            textureAppendageAri.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            textureAppendageMaddy.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            spriteHContent.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            System.Console.WriteLine("Loaded Bundle.");
        }


        /*
        void FetchObjects(GameObject _objID)
        {
            objectList = "<size=30>NEW LIST</size> \n";
            goNames = GameObject.FindObjectsOfType<GameObject>();

            listObjectDirectoryActive = new GameObject[_objID.transform.childCount - 1];
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

        */



        /*
        void NSFWSwitch(bool _nsfw)
        {
        }

        IEnumerator NSFWChecker()

        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                if(nsfwThing.adultActiveState)
                    NSFWSwitch(true);
                else
                    NSFWSwitch(false);
            }
        }*/



        public override void OnLateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                MelonLogger.Msg("TP " + currentCharacters.Length + " characters");
                byte id = 0;
                foreach (CharacterControllerBase _obj in currentCharacters)
                {
                    _obj.transform.rotation = Quaternion.identity;
                    _obj.transform.position = Camera.main.transform.position + Camera.main.transform.right * (id - 2) + Camera.main.transform.forward * 3 + Camera.main.transform.up * -.6f;
                    id++;
                }
            }


            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                foreach (CharacterControllerBase item in currentCharacters)
                {
                    item.Animator.SetBool("IdleA", false);
                    item.Animator.SetBool("SexA", true);
                    item.Animator.SetBool("SexCA", false);
                    item.Animator.SetBool("SexCB", false);
                    item.Animator.SetBool("SexCC", false);
                    item.Animator.SetBool("SexCD", false);
                    item.Animator.SetBool("SexSFW", false);
                }
            }


            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                foreach (CharacterControllerBase item in currentCharacters)
                {
                    item.Animator.SetBool("IdleA", false);
                    item.Animator.SetBool("SexA", false);
                    item.Animator.SetBool("SexCA", true);
                    item.Animator.SetBool("SexCB", false);
                    item.Animator.SetBool("SexCC", false);
                    item.Animator.SetBool("SexCD", false);
                    item.Animator.SetBool("SexSFW", false);





                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                foreach (CharacterControllerBase item in currentCharacters)
                {
                    item.Animator.SetBool("IdleA", false);
                    item.Animator.SetBool("SexA", false);
                    item.Animator.SetBool("SexCA", false);
                    item.Animator.SetBool("SexCB", true);
                    item.Animator.SetBool("SexCC", false);
                    item.Animator.SetBool("SexCD", false);
                    item.Animator.SetBool("SexSFW", false);
                }
            }


            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                foreach (CharacterControllerBase item in currentCharacters)
                {
                    item.Animator.SetBool("IdleA", false);
                    item.Animator.SetBool("SexA", false);
                    item.Animator.SetBool("SexCA", false);
                    item.Animator.SetBool("SexCB", false);
                    item.Animator.SetBool("SexCC", true);
                    item.Animator.SetBool("SexCD", false);
                    item.Animator.SetBool("SexSFW", false);
                }
            }


            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                foreach (CharacterControllerBase item in currentCharacters)
                {
                    item.Animator.SetBool("IdleA", false);
                    item.Animator.SetBool("SexA", false);
                    item.Animator.SetBool("SexCA", false);
                    item.Animator.SetBool("SexCB", false);
                    item.Animator.SetBool("SexCC", false);
                    item.Animator.SetBool("SexCD", true);
                    item.Animator.SetBool("SexSFW", false);
                }
            }


            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                foreach (CharacterControllerBase item in currentCharacters) 
                {
                    item.Animator.SetBool("IdleA", false);
                    item.Animator.SetBool("SexA", false);
                    item.Animator.SetBool("SexCA", false);
                    item.Animator.SetBool("SexCB", false);
                    item.Animator.SetBool("SexCC", false);
                    item.Animator.SetBool("SexCD", false);
                    item.Animator.SetBool("SexSFW", true); 
                }
            }


            if (Input.GetKeyDown(KeyCode.C))
            {
                foreach (CharacterControllerBase item in currentCharacters)
                {
                    MelonLogger.Msg(item.name+" :");
                    foreach (VariantConfig _va in item.CostumeSwitcher.variantConfigs)
                    MelonLogger.Msg(_va.variantName);
                    MelonLogger.Msg("");
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                foreach (CharacterControllerBase item in currentCharacters)
                    item.CostumeSwitcher.SwitchVariant("Nude");
            }


            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                foreach (CharacterControllerBase item in currentCharacters)
                    item.CostumeSwitcher.SwitchVariant("Hypnotized");
            }


            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                bool ai;
                Subscriber _dd = new Subscriber();
                SignalEvent  a= new SignalEvent();
                a.
                _dd.SetSignalEvent().AddListener(_dd.OnSignal);
                foreach (CharacterControllerBase item in currentCharacters)
                    item.CostumeSwitcher.SwitchVariant("Clothed");
            }

        }

        void Chipi()
        {

        }
    }
}
