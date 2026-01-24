using System;
using System.Runtime.CompilerServices;
using AdServices;
using System.Net.Http;
using System.Text.Json;

namespace Expenses;

internal class VersionAdditional
{
    private MVersion _mVersion = new();
    private VersionConfig _config;
    //todo:リリース時メンテ必須（カウントアップ）
    private int APPVERSION = 1;

    internal VersionAdditional()
    {
    }
    internal void Execute()
    {
        this.UpdateTable();
        this._mVersion.AppVersion = this.APPVERSION;
        this._mVersion.Update();
    }
    private void UpdateTable()
    {
        if (this._mVersion.TableVersion < 1)
        {
            TagCategory.AddColumn();
            this._mVersion.TableVersion = 1;
        }
    }
    internal bool CheckVersion()
    {
        if (this._config != null && this.APPVERSION < this._config.latestAppVersion)
        {
            return false;
        }
        else return true;
    }
    internal async Task SetConfig()
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            // オフライン時は何もしない（例外も出さない）
            return;
        }
        try
        {
            var url = "https://heybonk.github.io/app-version-config/version.json" + "?t=" + DateTimeOffset.UtcNow.ToUnixTimeSeconds(); ;
            var json = await new HttpClient().GetStringAsync(url);
            this._config = JsonSerializer.Deserialize<VersionConfig>(json);
        }
        catch
        {
            // 通信失敗してもアプリを落とさない
        }
    }
    internal void SetNowVersion()
    {
        this._mVersion = new MVersion() { CD = 1 };
        if (!this._mVersion.IsExistData())
        {
            this._mVersion.AppVersion = 0;
            this._mVersion.TableVersion = 0;
            this._mVersion.InsertData();
        }
        else
        {
            this._mVersion.SetByPrimaryKey();
        }
    }
}
public class VersionConfig
{
    public int latestAppVersion { get; set; }
}
