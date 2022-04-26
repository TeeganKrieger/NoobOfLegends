import React, { Component } from 'react';
import UserInfo from '../UserProfile/UserInfo'
import MatchList from '../UserProfile/MatchList'
import StatSelector from '../UserProfile/StatSelector'
import StatChart from './StatChart'
import GetColorSet from '../Helpers/DistinctColorGenerator'
import SkillDisplay from '../UserProfile/SkillDisplay';
import './UserComparison.css';

const PAGE_NAME = "UserComparison"

/* Component that renders an entire user profile using various sub components */
export default class UserComparison extends Component {

    constructor(props) {
        super(props);
        //let matches = props.additionalProps.matches;
        //let user = props.additionalProps.info;


        this.state = {
            user1Matches: [], user2Matches: [], user1Info: null, user2Info: null, selectedMatches: [], activeStat: { id: "gold", name: "Gold", lambda: (m) => m.gold }, skills: [],
            changePage: props.changePageFunc
        };
    }

    /* An array containing all tracked stats, their ids, names and lambda expressions */
    allStats = [
        { id: "gold", name: "Gold", lambda: (m) => m.gold },
        { id: "kills", name: "Kills", lambda: (m) => m.kills },
        { id: "deaths", name: "Deaths", lambda: (m) => m.deaths },
        { id: "assists", name: "Assists", lambda: (m) => m.assists },
        { id: "timeSpentDead", name: "Time Spent Dead", lambda: (m) => m.timeSpentDead },
        { id: "totalDamageDealt", name: "Total Damage Dealt", lambda: (m) => m.totalDamageDealt },
        { id: "baronKills", name: "Baron Kills", lambda: (m) => m.baronKills },
        { id: "dragonKills", name: "Dragon Kills", lambda: (m) => m.dragonKills },
        { id: "minionKills", name: "Minion Kills", lambda: (m) => m.minionKills },
        { id: "jungleMinionKills", name: "Jungle Minion Kills", lambda: (m) => m.jungleMinionKills },
        { id: "visionScore", name: "Vision Score", lambda: (m) => m.visionScore },
        { id: "killParticipation", name: "Kill Participation", lambda: (m) => m.killParticipation },
        { id: "healing", name: "Healing", lambda: (m) => m.healing },
    ]

    /* A lambda expression passed to StatSelector components to facilitate changing the selected stat */
    selectStatFunc = (stat) => {
        this.setState({ activeStat: stat });
    }

    /* A lambda expression passed to MatchListing components to facilitate selecting a match */
    selectMatch = (match) => {
        let selected = this.state.selectedMatches ?? [];
        this.setState({ selectedMatches: [] }); //Little fix for react error when modifying arrays within state
        selected.push(match);
        this.setState({ selectedMatches: selected });
    }

    /* A lambda expression passed to MatchListing components to facilitate unselecting a match */
    deselectMatch = (match) => {
        let matches = this.state.selectedMatches ?? [];
        let newMatches = matches.filter((m) => m.id != match.id);
        this.setState({ selectedMatches: newMatches });
    }

    /* A function that populates skills by fetching analysis from the backend */
    populateSkills() {
        console.log("Repopulating Skills");
        this.setState({ skills: this.GetFakeAnalysis() });
    }

    async handleSearchPlayer0(event) {
        event.preventDefault();

        var usernameAndTagline = document.getElementById("player-0-search").value;

        var user1Matches = this.state.user1Matches;
        var newSelectedMatches = [];
        
        this.state.selectedMatches.forEach(e => {
            if (!user1Matches.includes(e))
                newSelectedMatches.push(e);
        });

        this.setState({ user1Matches: [], user1Info: { username: usernameAndTagline.split("#")[0] }, selectedMatches: newSelectedMatches });

        if (usernameAndTagline != null && usernameAndTagline != "")
            this.fetchCheckpointId(usernameAndTagline, 0);
    }

    async handleSearchPlayer1(event) {
        event.preventDefault();

        var usernameAndTagline = document.getElementById("player-1-search").value;

        var user2Matches = this.state.user2Matches;
        var newSelectedMatches = [];

        this.state.selectedMatches.forEach(e => {
            if (!user2Matches.includes(e))
                newSelectedMatches.push(e);
        });

        this.setState({ user2Matches: [], user2Info: { username: usernameAndTagline.split("#")[0] }, selectedMatches: newSelectedMatches });


        if (usernameAndTagline != null && usernameAndTagline != "")
            this.fetchCheckpointId(usernameAndTagline, 1);
    }

    render() {

        var statSelectors = [];

        for (let i = 0; i < this.allStats.length; i++) {
            statSelectors.push(<StatSelector key={i} stat={this.allStats[i]} selectStatFunc={this.selectStatFunc} />)
        }

        return (
            <div className='container-fluid'>
                <div className='row'>
                    <div id='user-comparison-parent' className='col-12 col-md-7 col-xl-5'>
                        <form onSubmit={this.handleSearchPlayer0.bind(this)}>
                            <div className="input-group">
                                <input type="search" id="player-0-search" className="comparison-search" placeholder="Search for Player 1" aria-label="Search" />
                                <button id="player-0-submit" className="btn" type="submit">Search</button>
                            </div>
                        </form>
                        <MatchList matches={this.state.user1Matches} select={this.selectMatch} deselect={this.deselectMatch} />
                        <form onSubmit={this.handleSearchPlayer1.bind(this)}>
                            <div className="input-group">
                                <input type="search" id="player-1-search" className="comparison-search" placeholder="Search for Player 2" aria-label="Search" />
                                <button id="player-1-submit" className="btn" type="submit">Search</button>
                            </div>
                        </form>
                        <MatchList matches={this.state.user2Matches} select={this.selectMatch} deselect={this.deselectMatch} />
                    </div>
                    <div className='d-none d-xl-block col-xl-1'>
                    </div>
                    <div className='col-12 col-md-1'>
                        {statSelectors}
                    </div>
                    <div className='col-12 col-md-4'>
                        <StatChart stat={this.state.activeStat} matches={this.state.selectedMatches} />
                    </div>
                </div>
            </div>
        );
    }

    //Fetch
    /* Initiates a profile task on the backend and gets a checkpoint id. */
    async fetchCheckpointId(usernameAndTagline, playerNum) {

        let split = usernameAndTagline.split("#");

        if (split.length != 2) {
            const props = {
                "error": "BadRequest",
                "usernameAndTagline": usernameAndTagline
            };
            return;
        }

        //Make Request to backend for user. If User is found, redirect to other page
        const checkpointResponse = await fetch("api/user/start/" + split[0] + "/" + split[1], {
            method: 'GET',
            mode: 'no-cors',
        });

        if (checkpointResponse.status == 200) {

            const checkpointId = await checkpointResponse.text();

            console.log("Starting Loop");
            this.getCheckpointUpdate(checkpointId, playerNum);

        } else if (checkpointResponse.status == 400) {

            const props = {
                "error": "BadRequest",
                "usernameAndTagline": usernameAndTagline
            };

        } else {

            const props = {
                "error": "Server"
            };
        }
    }

    /* Uses a checkpoint id to recieve occasional updates from the backend to populate the page. */
    async getCheckpointUpdate(checkpointId, playerNum) {

        //Make Request to backend for user. If User is found, redirect to other page
        const checkpointResponse = await fetch("api/user/check/" + checkpointId, {
            method: 'GET',
            mode: 'no-cors',
        });

        if (checkpointResponse.status == 200) {
            const checkpoint = await checkpointResponse.json();

            if (checkpoint.error != 0) {
                const props = {
                    "error": "BadRequest",
                    "usernameAndTagline": ""
                };
            }

            if (playerNum == 0) {

                let currentMatches = this.state.user1Matches;
                currentMatches = currentMatches.concat(checkpoint.matches);

                let colorSeed = this.state.user1Info.username ?? "Default";
                let colors = GetColorSet(colorSeed, 100);

                for (let i = 0; i < currentMatches.length; i++)
                    currentMatches[i].color = colors[i];

                this.setState({ user1Matches: currentMatches });

            } else {

                let currentMatches = this.state.user2Matches;
                currentMatches = currentMatches.concat(checkpoint.matches);

                let colorSeed = this.state.user2Info.username ?? "Default";
                let colors = GetColorSet(colorSeed, 100);

                for (let i = 0; i < currentMatches.length; i++)
                    currentMatches[i].color = colors[i];

                this.setState({ user2Matches: currentMatches });

            }

            if (!checkpoint.completed) {
                setTimeout(await this.getCheckpointUpdate(checkpointId, playerNum), 2000);
            }

        } else if (checkpointResponse.status == 400) {

            const props = {
                "error": "BadRequest",
                "usernameAndTagline": ""
            };
        } else {
            //Show Network Error
            const props = {
                "error": "Server"
            };
        }

    }
}