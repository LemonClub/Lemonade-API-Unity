using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LA.User
{
    public struct UserInfo
    {
        public string   playerToken;    // 플레이어 토큰
        // _EDITION_0_
        public string   playerName;     // 플레이어 이름
        public int      playerAge;      // 플레이어 나이
        public string   playerSex;      // 플레이어 성별

        // _EDITION_1_
        public string   playerEmail;    // 플레이어 이메일
        public string   playerCP;       // 플레이어 전화번호
        public List<string> playerAchievement;  // 플레이어 업적
        public List<string> playerFollower;     // 플레이어 팔로워

        // _EDITION_2_
        public List<string> playerGames;        // 플레이어 타 게임
    }

    public class Lemon
    {
        public static LA.User.UserInfo _user;
    }
}
