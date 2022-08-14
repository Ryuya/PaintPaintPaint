//
// Copyright (c) 2020 Takap.
// This software is released under the MIT License.
//
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Audio;

    //
    // 1フレームに再生する効果音の数を1種類1つまでに制限する
    //  → 同じタイミングで同じSEを大量にPlayOneShotして爆音になるのを避ける
    //
    // すごく短い時間に要求された効果音はスケジュールして別のフレームで再生する
    //  → それでも最大数が存在する
    //

    /// <summary>
    /// <see cref="Addressables"/> を前提としたゲーム中のサウンド管理を管理するクラス
    /// </summary>
    public class SEPlayer : MonoBehaviour
    {
        //
        // InnerTypes
        // - - - - - - - - - - - - - - - - - - - -

        // 管理対象の効果音の情報
        public class _Info
        {
            // クリップの名前
            public string Name;
            // 再生済みかどうかのフラグ。true : 再生済み / false : まだ
            public bool IsDone;
            // 再生候補になってからの経過フレーム数
            public int FrameCount;
            // 再生する音
            public AudioClip Clip;
        }

        //
        // Constants
        // - - - - - - - - - - - - - - - - - - - -

        // 何でPlayPneshot()なのに複数チャンネル用意する必要があるんだろう？

        // SEチャンネル数
        public const int SE_CHANNEL = 1;
        // デフォルトのチャンネルを表す
        public const int SE_CH_DEFAULT = -1;

        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // 同じ音が同時再生数以上リクエストされてキューされた時に再生を遅延させるフレーム数
        [SerializeField, Range(1, 10)] private int delayFrameCount = 2;
        // SE再生予定キューに登録できる最大要素数
        [SerializeField, Range(1, 32)] private int maxQueudItemCount = 4;

        // SE再生用のオブジェクト(デフォルトch)
        [SerializeField, ReadOnly] private AudioSource defaultSource = default;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        //// 管理中のクリップ
        //private readonly List<_Info> clipList = new List<_Info>();

        private readonly Dictionary<string, Queue<_Info>> table = new Dictionary<string, Queue<_Info>>();

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// オーディオミキサーを設定します。
        /// </summary>
        public AudioMixerGroup AudioMixerGroup { set => this.defaultSource.outputAudioMixerGroup = value; }

        //
        // Runtime
        // - - - - - - - - - - - - - - - - - - - -

        protected void Update()
        {
            foreach (var q in this.table.Values)
            {
                if (q.Count == 0)
                {
                    continue;
                }

                while (true)
                {
                    if (q.Count == 0)
                    {
                        break;
                    }

                    if (q.Peek().IsDone)
                    {
                        var _ = q.Dequeue();
                    }
                    else
                    {
                        break;
                    }
                }

                if (q.Count == 0)
                {
                    continue;
                }

                // 未再生でキューの先頭の1件に対して
                var info = q.Peek();
                info.FrameCount++;
                if(info.FrameCount > this.delayFrameCount)
                {
                    this.defaultSource.PlayOneShot(info.Clip); // 時間が経過していたら再生して捨てる
                    var _ = q.Dequeue();
                }
            }

            // 全部再生し終わったらループを停止する
            if (this.count == 0)
            {
                this.table.Clear();
                this.enabled = false;
            }
        }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// オブジェクトを初期化し使用可能な状態にします。
        /// </summary>
        public void Setup()
        {
            if (!this.defaultSource)
            {
                var go = new GameObject("SE (default)");
                go.transform.parent = this.transform;
                this.defaultSource = go.AddComponent<AudioSource>();
            }
        }

        //
        // Private Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 効果音を再生します。
        /// </summary>
        public void PlaySE(string name, AudioClip clip)
        {
            // 再生要求があったときにループを開始し、それまでは開始しない
            this.enabled = true;

            var info = new _Info() { FrameCount = 0, Clip = clip, };

            if (!this.table.ContainsKey(name))
            {
                this.defaultSource.PlayOneShot(clip);
                info.IsDone = true;

                var q = new Queue<_Info>();
                q.Enqueue(info);
                this.table[name] = q;
            }
            else
            {
                var list = this.table[name];
                if (list.Count <= this.maxQueudItemCount)
                {
                    this.table[name].Enqueue(info);
                }
                else
                {
                    Debug.Log($"効果音の最大登録数を超えています。name={name}");
                }
            }
        }

        // 有効な要素数を取得する
        private int count
        {
            get
            {
                int num = 0;
                foreach (var list in this.table.Values)
                {
                    num += list.Count;
                }
                return num;
            }
        }
    }