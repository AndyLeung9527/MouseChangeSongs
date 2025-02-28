using Gma.System.MouseKeyHook;
using System.Runtime.InteropServices;

namespace MouseChangeSongs_Global;

public partial class Main : Form
{
    private bool _active;
    private IKeyboardMouseEvents _globalHook;

    public Main()
    {
        InitializeComponent();

        label1.Text = $"开始监听后鼠标侧键切换歌曲{Environment.NewLine}©2025 LWB";

        _active = false;
        button1.Text = "开始监听";

        _globalHook = Hook.GlobalEvents();

        notifyIcon1.Text = "ChangeSongs";
        notifyIcon1.Visible = true;
        notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
        notifyIcon1.ContextMenuStrip = contextMenuStrip1;

        contextMenuStrip1.Items.Add("退出", null, (s, e) => { Application.Exit(); });
    }

    private void button1_Click(object sender, EventArgs e)
    {
        _active = !_active;
        if (_active)
        {

            button1.Text = "停止监听";
            _globalHook.MouseDownExt += M_GlobalHook_MouseDownExt;
        }
        else
        {
            button1.Text = "开始监听";
            _globalHook.MouseDownExt -= M_GlobalHook_MouseDownExt;
        }
    }

    private void Main_SizeChanged(object sender, EventArgs e)
    {
        if (this.WindowState == FormWindowState.Minimized)
        {
            this.Hide();
        }
    }

    private void notifyIcon1_MouseDoubleClick(object? sender, MouseEventArgs e)
    {
        this.Visible = true;
        this.WindowState = FormWindowState.Normal;
        this.Activate();
    }

    private void M_GlobalHook_MouseDownExt(object? sender, MouseEventExtArgs e)
    {
        switch (e.Button)
        {
            case MouseButtons.XButton1:
                keybd_event(Keys.MediaNextTrack, 0, 0, 0);
                break;
            case MouseButtons.XButton2:
                keybd_event(Keys.MediaPreviousTrack, 0, 0, 0);
                break;
            default:
                break;
        }
    }

    [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
    public static extern void keybd_event(Keys bVk, byte bScan, uint dwFlags, uint dwExtraInfo);
}
