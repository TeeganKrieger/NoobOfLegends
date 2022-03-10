import React, { Component } from 'react';
import './UserProfile.css';
import UserInfo from './UserInfo'
import MatchList from './MatchList'
import StatSelector from './StatSelector'
import Comparison from './Comparison'

export default class UserProfile extends Component {

    constructor(props) {
        super(props);
        this.state = { allMatches: this.GetFakeMatches(), selectedMatches: [], activeStatName: "gold", activeStatLambda: (m) => m.gold, };
    }

    selectStatFunc = (name, lambda) => {
        this.setState({ activeStatName: name, activeStatLambda: lambda });
    }

    selectMatch = (match) => {
        let newMatches = this.state.selectedMatches.push(match);
        this.setState({ selectedMatches: newMatches });
    }

    deselectMatch = (match) => {
        let matches = this.state.selectedMatches;
        let newMatches = matches.filter((m) => m.id != match.id);
        this.setState({ selectedMatches: newMatches });
    }

    render() {

        return (
            <div className='container-fluid'>
                <div className='row'>
                    <div className='col-12 col-md-5'>
                        <UserInfo user={this.GetFakeUserInfo()} />
                        <MatchList matches={this.GetFakeMatches()} select={this.selectMatch} deselect={this.deselectMatch} />
                    </div>
                    <div className='col-12 col-md-2'>
                    </div>
                    <div className='col-12 col-md-1'>
                        <StatSelector stat="gold" lambda={(m) => m.gold} selectStat={this.selectStatFunc} />
                        <StatSelector stat="kills" lambda={(m) => m.kills} selectStat={this.selectStatFunc} />
                        <StatSelector stat="deaths" lambda={(m) => m.deaths} selectStat={this.selectStatFunc} />
                        <StatSelector stat="assists" lambda={(m) => m.assists} selectStat={this.selectStatFunc} />
                        <StatSelector stat="timeSpentDead" lambda={(m) => m.timeSpentDead} selectStat={this.selectStatFunc} />
                        <StatSelector stat="totalDamageDealt" lambda={(m) => m.totalDamageDealt} selectStat={this.selectStatFunc} />
                        <StatSelector stat="baronKills" lambda={(m) => m.baronKills} selectStat={this.selectStatFunc} />
                        <StatSelector stat="dragonKills" lambda={(m) => m.dragonKills} selectStat={this.selectStatFunc} />
                        <StatSelector stat="minionKills" lambda={(m) => m.minionKills} selectStat={this.selectStatFunc} />
                        <StatSelector stat="jungleMinionKills" lambda={(m) => m.jungleMinionKills} selectStat={this.selectStatFunc} />
                        <StatSelector stat="visionScore" lambda={(m) => m.visionScore} selectStat={this.selectStatFunc} />
                        <StatSelector stat="killParticipation" lambda={(m) => m.killParticipation} selectStat={this.selectStatFunc} />
                        <StatSelector stat="healing" lambda={(m) => m.healing} selectStat={this.selectStatFunc} />
                    </div>
                    <div className='col-12 col-md-4'>
                        <Comparison name={this.state.activeStatName} matches={this.GetFakeMatches()} lambda={this.state.activeStatLambda} />
                    </div>
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
                "id": "NA_1",
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
                "id": "NA_2",
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
                "id": "NA_3",
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
                "id": "NA_4",
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
                "id": "NA_5",
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
                "id": "NA_6",
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