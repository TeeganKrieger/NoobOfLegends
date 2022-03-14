import React, { Component } from 'react';
import './SkillDisplay.css';

export default class SkillDisplay extends Component {

    constructor(props) {
        super(props);
        this.state = {
            skills: props.skills
        };
    }

    componentWillReceiveProps(props) {
        this.state = { skills: props.skills };
    }

    render() {

        let skills = this.state.skills;

        let rows = [];

        let curr = [];

        for (let i = 0; i < skills.length; i++) {
            if (i > 0 && i % 4 == 0) {
                rows.push(<div key={(i / 4) + "_row "} className="row">{curr}</div>);
                curr = [];
            }

            curr.push(
                <div key={i + "_ele"} className="col-3">
                    <div className={"skill " + (skills[i].good ? "good" : "bad")}>
                        <span>{skills[i].name}</span>
                    </div>
                </div>
            );
        }
        rows.push(<div key="last_row" className="row">{curr}</div>);

        return (
            <div>
                {rows}
            </div>
        );
    }
}