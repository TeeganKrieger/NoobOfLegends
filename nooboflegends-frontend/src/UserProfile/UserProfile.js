import React, { Component } from 'react';
import './UserProfile.css';
import UserInfo from './UserInfo'
import MatchList from './MatchList'

export default class UserProfile extends Component {

    constructor(props) {
        super(props);
        this.state = {};
    }

    render() {

        return (
            <div className='row'>
                <div className='col-12 col-md-5'>
                    <UserInfo user={this.GetFakeUserInfo()} />
                    <MatchList matches={this.GetFakeMatches()} />
                </div>
                <div className='col-md-2'>

                </div>
                <div className='col-md-5'>

                </div>
            </div>
            );
    }

    //Testing
    GetFakeUserInfo() {
        return {
            "username": "Saltymate8",
            "tagline": "NA1",
            "rankFlex": {
                "rank": 7,
                "tier": 0,
                "lp": 45,
            },
            "rankSoloDuo": {
                "rank": 5,
                "tier": 3,
                "lp": 98,
            }
        };
    }

    GetFakeMatches() {
        return [
                {
                    "queue": 0,
                    "champion": "Zeri",
                    "position": "MID",
                    "averageRank": 6,
                    "won": true,
                    "playedOn": 192883851,
                    "kills": 7,
                    "deaths": 12,
                    "assists": 6
                },
                {
                    "queue": 1,
                    "champion": "DrMundo",
                    "position": "MID",
                    "averageRank": 0,
                    "won": false,
                    "playedOn": 21847214,
                    "kills": 12,
                    "deaths": 7,
                    "assists": 3
                },
                {
                    "queue": 0,
                    "champion": "MonkeyKing",
                    "position": "BOT",
                    "averageRank": 7,
                    "won": false,
                    "playedOn": 312832111,
                    "kills": 9,
                    "deaths": 2,
                    "assists": 4
                },
                {
                    "queue": 0,
                    "champion": "Zeri",
                    "position": "MID",
                    "averageRank": 6,
                    "won": true,
                    "playedOn": 192883851,
                    "kills": 7,
                    "deaths": 12,
                    "assists": 6
                },
                {
                    "queue": 1,
                    "champion": "DrMundo",
                    "position": "MID",
                    "averageRank": 0,
                    "won": false,
                    "playedOn": 21847214,
                    "kills": 12,
                    "deaths": 7,
                    "assists": 3
                },
                {
                    "queue": 0,
                    "champion": "MonkeyKing",
                    "position": "BOT",
                    "averageRank": 7,
                    "won": false,
                    "playedOn": 312832111,
                    "kills": 9,
                    "deaths": 2,
                    "assists": 4
                },
                {
                    "queue": 0,
                    "champion": "Zeri",
                    "position": "MID",
                    "averageRank": 6,
                    "won": true,
                    "playedOn": 192883851,
                    "kills": 7,
                    "deaths": 12,
                    "assists": 6
                },
                {
                    "queue": 1,
                    "champion": "DrMundo",
                    "position": "MID",
                    "averageRank": 0,
                    "won": false,
                    "playedOn": 21847214,
                    "kills": 12,
                    "deaths": 7,
                    "assists": 3
                },
                {
                    "queue": 0,
                    "champion": "MonkeyKing",
                    "position": "BOT",
                    "averageRank": 7,
                    "won": false,
                    "playedOn": 312832111,
                    "kills": 9,
                    "deaths": 2,
                    "assists": 4
                },
                {
                    "queue": 0,
                    "champion": "Zeri",
                    "position": "MID",
                    "averageRank": 6,
                    "won": true,
                    "playedOn": 192883851,
                    "kills": 7,
                    "deaths": 12,
                    "assists": 6
                },
                {
                    "queue": 1,
                    "champion": "DrMundo",
                    "position": "MID",
                    "averageRank": 0,
                    "won": false,
                    "playedOn": 21847214,
                    "kills": 12,
                    "deaths": 7,
                    "assists": 3
                },
                {
                    "queue": 0,
                    "champion": "MonkeyKing",
                    "position": "BOT",
                    "averageRank": 7,
                    "won": false,
                    "playedOn": 312832111,
                    "kills": 9,
                    "deaths": 2,
                    "assists": 4
                },
                {
                    "queue": 0,
                    "champion": "Zeri",
                    "position": "MID",
                    "averageRank": 6,
                    "won": true,
                    "playedOn": 192883851,
                    "kills": 7,
                    "deaths": 12,
                    "assists": 6
                },
                {
                    "queue": 1,
                    "champion": "DrMundo",
                    "position": "MID",
                    "averageRank": 0,
                    "won": false,
                    "playedOn": 21847214,
                    "kills": 12,
                    "deaths": 7,
                    "assists": 3
                },
                {
                    "queue": 0,
                    "champion": "MonkeyKing",
                    "position": "BOT",
                    "averageRank": 7,
                    "won": false,
                    "playedOn": 312832111,
                    "kills": 9,
                    "deaths": 2,
                    "assists": 4
                }
            ];
    }

}