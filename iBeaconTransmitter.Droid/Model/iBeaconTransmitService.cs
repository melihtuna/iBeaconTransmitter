﻿using System;
using iBeaconTransmitter.Model;
using iBeaconTransmitter.Droid.Model;
using Xamarin.Forms;
using AltBeaconOrg.BoundBeacon;

[assembly: Dependency(typeof(iBeaconTransmitService))]
namespace iBeaconTransmitter.Droid.Model
{
	public class iBeaconTransmitService : IiBeaconTransmitService
	{
		#region Constant

		/// <summary>
		/// iBeaconのバイト列のフォーマット
		/// m：ビーコンのタイプ
		/// i：identifier（UUID、Major、Minorの場所の指定）
		/// p：パワーキャリブレーションの値
		/// </summary>
		const string IBEACON_FORMAT = "m:2-3=0215,i:4-19,i:20-21,i:22-23,p:24-24";

		#endregion

		#region Variables

		/// <summary>
		/// ビーコンの送信を制御するクラスのインスタンス
		/// </summary>
		BeaconTransmitter _beaconTransmitter;

		#endregion

		#region Public methods

		/// <summary>
		/// iBeacon発信開始処理（引数で渡されたiBeaconの定義に従い発信）
		/// </summary>
		/// <param name="ibeacon">iBeaconの定義</param>
		public void StartTransmission(iBeacon ibeacon)
		{
			this.StartTransmission(ibeacon.Uuid, ibeacon.Major, ibeacon.Minor);
		}

		/// <summary>
		/// iBeacon発信開始処理（UUID、Major、Minorを直指定して発信）
		/// </summary>
		/// <param name="uuid">UUID</param>
		/// <param name="major">Major</param>
		/// <param name="minor">Minor</param>
		public void StartTransmission(Guid uuid, ushort major, ushort minor)
		{
			// Android用のビーコン定義を作成
			Beacon beacon = new Beacon.Builder()
			                    .SetId1(uuid.ToString())
			                    .SetId2(major.ToString())
			                    .SetId3(minor.ToString())
			                    .SetManufacturer(0x004C)
			                    .Build();

			// iBeaconのバイト列フォーマットをBeaconParser（アドバタイズ時のバイト列定義）にセットする。
			BeaconParser beaconParser = new BeaconParser().SetBeaconLayout(IBEACON_FORMAT);

			// iBeaconの発信を開始する。
			_beaconTransmitter = new BeaconTransmitter(Android.App.Application.Context, beaconParser);
			_beaconTransmitter.StartAdvertising(beacon);
		}

		/// <summary>
		/// iBeaconの発信を停止する処理
		/// </summary>
		public void StopTransmission()
		{
			_beaconTransmitter.StopAdvertising();
		}

		#endregion
	}
}