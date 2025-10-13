using Il2CppTMPro;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace LastEpoch_Hud.Scripts
{
    [RegisterTypeInIl2Cpp]
    public class VirtualKeyboard : MonoBehaviour
    {
        public VirtualKeyboard(System.IntPtr ptr) : base(ptr) { }
        public static VirtualKeyboard instance;
        
        public static bool Initializing = false;
        public static GameObject Virtual_Keyboard_prefab = null;
        public static GameObject Virtual_Keyboard = null;
        public static string Name = "Hud_VirtualKeyboard";
        public static TMP_InputField InputField = null;

        void Awake()
        {
            instance = this;
        }
        void Update()
        {
            if (!Assets.Loaded()) { Assets.Load(); }
            else if (!Virtual_Keyboard) { Init(); }
        }
        void Init()
        {
            if (!Initializing)
            {
                Initializing = true;
                Virtual_Keyboard = Instantiate(Virtual_Keyboard_prefab, Vector3.zero, Quaternion.identity);
                Virtual_Keyboard.name = Name;
                if (!Virtual_Keyboard.IsNullOrDestroyed())
                {
                    GameObject row_0 = Functions.GetChild(Virtual_Keyboard, "Row_0");
                    if (!row_0.IsNullOrDestroyed())
                    {
                        Buttons.A = Buttons.Get(row_0, "A");
                        Events.Set(Buttons.A, Events.A_OnClick_Action);

                        Buttons.Z = Buttons.Get(row_0, "Z");
                        Events.Set(Buttons.Z, Events.Z_OnClick_Action);

                        Buttons.E = Buttons.Get(row_0, "E");
                        Events.Set(Buttons.E, Events.E_OnClick_Action);

                        Buttons.R = Buttons.Get(row_0, "R");
                        Events.Set(Buttons.R, Events.R_OnClick_Action);

                        Buttons.T = Buttons.Get(row_0, "T");
                        Events.Set(Buttons.T, Events.T_OnClick_Action);

                        Buttons.Y = Buttons.Get(row_0, "Y");
                        Events.Set(Buttons.Y, Events.Y_OnClick_Action);

                        Buttons.U = Buttons.Get(row_0, "U");
                        Events.Set(Buttons.U, Events.U_OnClick_Action);

                        Buttons.I = Buttons.Get(row_0, "I");
                        Events.Set(Buttons.I, Events.I_OnClick_Action);

                        Buttons.O = Buttons.Get(row_0, "O");
                        Events.Set(Buttons.O, Events.O_OnClick_Action);

                        Buttons.P = Buttons.Get(row_0, "P");
                        Events.Set(Buttons.P, Events.P_OnClick_Action);
                    }
                    GameObject row_1 = Functions.GetChild(Virtual_Keyboard, "Row_1");
                    if (!row_1.IsNullOrDestroyed())
                    {
                        Buttons.Q = Buttons.Get(row_1, "Q");
                        Events.Set(Buttons.Q, Events.Q_OnClick_Action);

                        Buttons.S = Buttons.Get(row_1, "S");
                        Events.Set(Buttons.S, Events.S_OnClick_Action);

                        Buttons.D = Buttons.Get(row_1, "D");
                        Events.Set(Buttons.D, Events.D_OnClick_Action);

                        Buttons.F = Buttons.Get(row_1, "F");
                        Events.Set(Buttons.F, Events.F_OnClick_Action);

                        Buttons.G = Buttons.Get(row_1, "G");
                        Events.Set(Buttons.G, Events.G_OnClick_Action);

                        Buttons.H = Buttons.Get(row_1, "H");
                        Events.Set(Buttons.H, Events.H_OnClick_Action);

                        Buttons.J = Buttons.Get(row_1, "J");
                        Events.Set(Buttons.J, Events.J_OnClick_Action);

                        Buttons.K = Buttons.Get(row_1, "K");
                        Events.Set(Buttons.K, Events.K_OnClick_Action);

                        Buttons.L = Buttons.Get(row_1, "L");
                        Events.Set(Buttons.L, Events.L_OnClick_Action);

                        Buttons.M = Buttons.Get(row_1, "M");
                        Events.Set(Buttons.M, Events.M_OnClick_Action);
                    }
                    GameObject row_2 = Functions.GetChild(Virtual_Keyboard, "Row_2");
                    if (!row_2.IsNullOrDestroyed())
                    {
                        Buttons.W = Buttons.Get(row_2, "W");
                        Events.Set(Buttons.W, Events.W_OnClick_Action);

                        Buttons.X = Buttons.Get(row_2, "X");
                        Events.Set(Buttons.X, Events.X_OnClick_Action);

                        Buttons.C = Buttons.Get(row_2, "C");
                        Events.Set(Buttons.C, Events.C_OnClick_Action);

                        Buttons.V = Buttons.Get(row_2, "V");
                        Events.Set(Buttons.V, Events.V_OnClick_Action);

                        Buttons.B = Buttons.Get(row_2, "B");
                        Events.Set(Buttons.B, Events.B_OnClick_Action);

                        Buttons.N = Buttons.Get(row_2, "N");
                        Events.Set(Buttons.N, Events.N_OnClick_Action);

                        Buttons.Return = Buttons.Get(row_2, "Return");
                        Events.Set(Buttons.Return, Events.Return_OnClick_Action);

                        Buttons.Space = Buttons.Get(row_2, "Space");
                        Events.Set(Buttons.Space, Events.Space_OnClick_Action);
                    }
                }
                Initializing = false;
            }            
        }

        public void MoveTo(GameObject go, TMP_InputField inputfield)
        {
            if ((!Virtual_Keyboard.IsNullOrDestroyed()) && (!go.IsNullOrDestroyed())) // && (!InputField.IsNullOrDestroyed()))
            {
                GameObject vk = Functions.GetChild(go, Name);
                if (vk.IsNullOrDestroyed()) //Used to check if already exist
                {
                    Virtual_Keyboard.transform.SetParent(go.transform);
                    Virtual_Keyboard.transform.localScale = new Vector3(1, 1, 1);
                    InputField = inputfield;
                }
            }
        }

        public class Assets
        {
            private static bool loading = false;

            public static bool Loaded()
            {
                bool result = false;
                if (!Virtual_Keyboard_prefab.IsNullOrDestroyed())
                {
                    result = true;
                }

                return result;
            }
            public static void Load()
            {
                if (!Hud_Manager.asset_bundle.IsNullOrDestroyed() && !loading)
                {
                    loading = true;
                    foreach (string name in Hud_Manager.asset_bundle.GetAllAssetNames())
                    {
                        if ((Functions.Check_Prefab(name)) && (name.Contains("/virtualkeyboard/")) && (name.Contains("virtualkeyboard.prefab")))
                        {
                            Virtual_Keyboard_prefab = Hud_Manager.asset_bundle.LoadAsset(name).TryCast<GameObject>();
                        }
                    }
                    loading = false;
                }
            }
        }
        public class Events
        {
            public static void Set(Button btn, UnityEngine.Events.UnityAction action)
            {
                if (!btn.IsNullOrDestroyed())
                {
                    btn.onClick = new Button.ButtonClickedEvent();
                    btn.onClick.AddListener(action);
                }
            }

            public static readonly System.Action A_OnClick_Action = new System.Action(A_Click);
            public static void A_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "a"; }
            }

            public static readonly System.Action B_OnClick_Action = new System.Action(B_Click);
            public static void B_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "b"; }
            }

            public static readonly System.Action C_OnClick_Action = new System.Action(C_Click);
            public static void C_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "c"; }
            }

            public static readonly System.Action D_OnClick_Action = new System.Action(D_Click);
            public static void D_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "d"; }
            }

            public static readonly System.Action E_OnClick_Action = new System.Action(E_Click);
            public static void E_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "e"; }
            }

            public static readonly System.Action F_OnClick_Action = new System.Action(F_Click);
            public static void F_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "f"; }
            }

            public static readonly System.Action G_OnClick_Action = new System.Action(G_Click);
            public static void G_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "g"; }
            }

            public static readonly System.Action H_OnClick_Action = new System.Action(H_Click);
            public static void H_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "h"; }
            }

            public static readonly System.Action I_OnClick_Action = new System.Action(I_Click);
            public static void I_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "i"; }
            }

            public static readonly System.Action J_OnClick_Action = new System.Action(J_Click);
            public static void J_Click()
            {                
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "j"; }
            }

            public static readonly System.Action K_OnClick_Action = new System.Action(K_Click);
            public static void K_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "k"; }
            }

            public static readonly System.Action L_OnClick_Action = new System.Action(L_Click);
            public static void L_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "l"; }
            }

            public static readonly System.Action M_OnClick_Action = new System.Action(M_Click);
            public static void M_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "m"; }
            }

            public static readonly System.Action N_OnClick_Action = new System.Action(N_Click);
            public static void N_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "n"; }
            }

            public static readonly System.Action O_OnClick_Action = new System.Action(O_Click);
            public static void O_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "o"; }
            }

            public static readonly System.Action P_OnClick_Action = new System.Action(P_Click);
            public static void P_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "p"; }
            }

            public static readonly System.Action Q_OnClick_Action = new System.Action(Q_Click);
            public static void Q_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "q"; }
            }

            public static readonly System.Action R_OnClick_Action = new System.Action(R_Click);
            public static void R_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "r"; }
            }

            public static readonly System.Action S_OnClick_Action = new System.Action(S_Click);
            public static void S_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "s"; }
            }

            public static readonly System.Action T_OnClick_Action = new System.Action(T_Click);
            public static void T_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "t"; }
            }

            public static readonly System.Action U_OnClick_Action = new System.Action(U_Click);
            public static void U_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "u"; }
            }

            public static readonly System.Action V_OnClick_Action = new System.Action(V_Click);
            public static void V_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "v"; }
            }

            public static readonly System.Action W_OnClick_Action = new System.Action(W_Click);
            public static void W_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "w"; }
            }

            public static readonly System.Action X_OnClick_Action = new System.Action(X_Click);
            public static void X_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "x"; }
            }

            public static readonly System.Action Y_OnClick_Action = new System.Action(Y_Click);
            public static void Y_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "y"; }
            }

            public static readonly System.Action Z_OnClick_Action = new System.Action(Z_Click);
            public static void Z_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += "z"; }
            }

            public static readonly System.Action Return_OnClick_Action = new System.Action(Return_Click);
            public static void Return_Click()
            {
                if (!InputField.IsNullOrDestroyed())
                {
                    string result = "";
                    int i = 0;
                    foreach (char c in InputField.text.ToCharArray())
                    {
                        if (i < InputField.text.ToCharArray().Length - 1) { result += c; }                        
                        i++;
                    }
                    InputField.text = result;
                }
            }

            public static readonly System.Action Space_OnClick_Action = new System.Action(Space_Click);
            public static void Space_Click()
            {
                if (!InputField.IsNullOrDestroyed()) { InputField.text += " "; }
            }
        }
        public class Buttons
        {
            public static Button A = null;
            public static Button B = null;
            public static Button C = null;
            public static Button D = null;
            public static Button E = null;
            public static Button F = null;
            public static Button G = null;
            public static Button H = null;
            public static Button I = null;
            public static Button J = null;
            public static Button K = null;
            public static Button L = null;
            public static Button M = null;
            public static Button N = null;
            public static Button O = null;
            public static Button P = null;
            public static Button Q = null;
            public static Button R = null;
            public static Button S = null;
            public static Button T = null;
            public static Button U = null;
            public static Button V = null;
            public static Button W = null;
            public static Button X = null;
            public static Button Y = null;
            public static Button Z = null;
            public static Button Return = null;
            public static Button Space = null;

            public static Button Get(GameObject go, string btn_name)
            {
                Button btn = null;
                GameObject btn_obj = Functions.GetChild(go, btn_name);
                if (!btn_obj.IsNullOrDestroyed()) { btn = btn_obj.GetComponent<Button>(); }

                return btn;
            }
        }
    }
}
