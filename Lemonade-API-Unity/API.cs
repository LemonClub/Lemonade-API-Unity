using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace LA
{
    public class API
    {
        public delegate void initDelegate(LA.User.UserInfo dicArg);

        /// <summary>
        /// 암호화 해독
        /// </summary>
        /// <param name="dic">return 할 Dictionary 의 레퍼런스</param>
        /// <param name="w">WWW의 결과 값</param>
        void decryption(ref Dictionary<string, string> dic, WWW w)
        {
            bool readStart = false;         // 읽기 시작함을 알림 '{' 를 만났을 때 사용
            bool reading = true;            // true : 이름,  false : 내용

            string dbName = "";             // db 의 이름
            string dbContent = "";          // db 의 내용

            int i = 0;

            while (i < w.text.Length)
            {
                if (readStart)
                {
                    if (reading)
                    {
                        if (w.text[i].Equals(":"))
                            reading = false;
                        else
                            dbName += w.text[i];
                    }
                    else
                    {
                        if (w.text[i].Equals("/"))
                        {
                            dbName = "";
                            dic.Add(dbName, dbContent);
                        }
                        else
                        {
                            dbContent += w.text[i];
                        }
                    }

                    if (w.text[i].Equals("}"))          // 끝을 알림
                        break;
                }
                else if (w.text[i].Equals("{"))          // 시작을 알림
                {
                    readStart = true;
                }
            }
        }

        /// <summary>
        /// 사용자 정보 받아오기 ( 앱에 로그인 되어 있는 유저를 말함 )
        /// </summary>
        /// <param name="userToken">유저 토큰</param>
        /// <param name="func">성공적으로 마쳤을때 불러올 함수</param>
        /* public IEnumerator getPlayerInfo(initDelegate func)
        {
            // GAME.init 이 true 가 되기보다 getPlayerInfo 더 빠를수도 있음을 방지
            while (!GAME.init)
                yield return null;

            if (GAME.init)
            {
                WWWForm form = new WWWForm();
                form.AddField("game_stage", GAME.game_stage);
                form.AddField("user_token", LA.User.Lemon._user.playerToken);
                WWW w = new WWW("http://lemontree.dothome.co.kr/get_user_info", form);

                yield return w;

                if (!w.text.Equals("NOT FOUND"))
                {
                    LA.LAData data = JsonUtility.FromJson<LA.LAData>(w.text);

                    // _EDITION_0_
                    LA.User.Lemon._user.playerToken = data.LA_USER_TOKEN;
                    LA.User.Lemon._user.playerName = data.LA_USER_NAME;
                    LA.User.Lemon._user.playerAge = System.Convert.ToInt32(data.LA_USER_AGE);
                    if (data.LA_USER_SEX.Equals("0"))
                        LA.User.Lemon._user.playerSex = "MAN";
                    else LA.User.Lemon._user.playerSex = "WOMAN";
                    // _EDITION_1_
                    LA.User.Lemon._user.playerEmail = data.LA_USER_EMAIL;
                    LA.User.Lemon._user.playerCP = data.LA_USER_PHONE;
                    LA.User.Lemon._user.playerAchievement = data.LA_USER_ACHIEVEMENT;
                    LA.User.Lemon._user.playerFollower = data.LA_USER_FOLLOWER;
                    // _EDITION_2_
                    LA.User.Lemon._user.playerGames = data.LA_USER_GAMES;

                    func(LA.User.Lemon._user);
                }
            }
        } */

        /// <summary>
        /// 유저 정보 받아오기 ( 어느 유저 타겟에 대한 정보 )
        /// </summary>
        /// <param name="userToken">유저 토큰</param>
        /// <param name="func">성공적으로 마쳤을때 불러올 함수</param>
        public IEnumerator getUserInfo(string userToken, initDelegate func)
        {
            // GAME.init 이 true 가 되기보다 getUserInfo호출이 더 빠를수도 있음을 방지
            while (!GAME.init)
                yield return null;

            if (GAME.init)
            {
                WWWForm form = new WWWForm();
                form.AddField("game_stage", GAME.game_stage);
                form.AddField("user_token", userToken);
                WWW w = new WWW("http://lemontree.dothome.co.kr/get_user_info", form);

                yield return w;

                if (!w.text.Equals(""))
                {
                    LA.User.UserInfo uinfo = new LA.User.UserInfo();
                    LA.LAData data = JsonUtility.FromJson<LA.LAData>(w.text);

                    // _EDITION_0_
                    uinfo.playerToken = data.LA_USER_TOKEN;
                    uinfo.playerName = data.LA_USER_NAME;
                    uinfo.playerAge = System.Convert.ToInt32(data.LA_USER_AGE);
                    if (data.LA_USER_SEX.Equals("0")) uinfo.playerSex = "MAN";
                    else uinfo.playerSex = "WOMAN";
                    // _EDITION_1_
                    uinfo.playerEmail = data.LA_USER_EMAIL;
                    uinfo.playerCP = data.LA_USER_PHONE;
                    uinfo.playerAchievement = data.LA_USER_ACHIEVEMENT;
                    uinfo.playerFollower = data.LA_USER_FOLLOWER;
                    // _EDITION_2_
                    uinfo.playerGames = data.LA_USER_GAMES;

                    func(uinfo);
                }
                else { Debug.Log("w Error"); }
            }
        }

        public IEnumerator getDatabaseC(string lStr, string userToken)
        {
            while (!GAME.init)
                yield return null;

            if (GAME.init)
            {
                WWWForm form = new WWWForm();
                form.AddField("token", GAME.game_token);
                form.AddField("lStr", Regex.Replace(lStr, @"[^a-zA-Z0-9*, ]", "", RegexOptions.Singleline));
                form.AddField("user", Regex.Replace(userToken, @"[^a-zA-Z0-9=]", "", RegexOptions.Singleline));

                WWW w = new WWW("http://lemontree.dothome.co.kr/dbCustom", form);

                yield return w;

                Debug.Log("Custom Database JSON Return : " + w.text);
            }
        }

        /// <summary>
        /// 게임 초기화
        /// </summary>
        /// <remarks>'자동 로그인'을 풀기 위함</remarks>
        /// <param name="token">게임 토큰</param>
        public IEnumerator init(string token)
        {
            GAME.game_token = token;

            // 게임 보안 단계를 알아오는 중
            WWWForm form = new WWWForm();
            form.AddField("token", GAME.game_token);
            WWW w = new WWW("http://lemontree.dothome.co.kr/get_game_stage", form);

            yield return w;
            GAME.init = true;
            GAME.game_stage = System.Convert.ToInt32(w.text);
        }
    }
}
