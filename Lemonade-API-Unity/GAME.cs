using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LA
{
    static class GAME
    {
        public static bool init = false;       // 게임 초기화 한번만 이루어 지게만 함 ( 중복 토큰을 이용한 악용 방지 )

        public static string game_token = "";         // 게임 토큰
        public static int game_stage = 0;             // 게임 보안 단계
    }

    class LAData
    {
        // _EDITION_0_
        public string LA_USER_TOKEN;
        public string LA_USER_NAME;
        public string LA_USER_AGE;
        public string LA_USER_SEX;

        // _EDITION_1_
        public string LA_USER_EMAIL;
        public string LA_USER_PHONE;
        public List<string> LA_USER_ACHIEVEMENT;
        public List<string> LA_USER_FOLLOWER;

        // _EDITION_2_
        public List<string> LA_USER_GAMES;
    }
}
