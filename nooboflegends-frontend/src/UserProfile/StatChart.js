import React, { Component } from 'react';
import './StatChart.css';
import {
    Chart as ChartJS,
    CategoryScale,
    LinearScale,
    BarElement,
    Title,
    Tooltip,
    Legend,
} from 'chart.js';
import { Bar } from 'react-chartjs-2';

export default class StatChart extends Component {

    constructor(props) {
        super(props);
        this.state = { stat: props.stat, matches: props.matches };

        ChartJS.register(
            CategoryScale,
            LinearScale,
            BarElement,
            Title,
            Tooltip,
            Legend
        );
    }

    componentWillReceiveProps(props) {
        this.state = { stat: props.stat, matches: props.matches };
    }

    render() {
        let stat = this.state.stat;
        let matches = this.state.matches;

        let vals = [];
        let labels = [];
        let colors = [];

        for (let i = 0; i < matches.length; i++) {
            let val = stat.lambda(matches[i]);
            vals.push(val);
            labels.push("Match " + (i + 1));
            colors.push(matches[i].color);
        }

        let height = Math.max(450, 50 * vals.length);

        let options = {
            indexAxis: 'y',
            maintainAspectRatio: false,
            responsive: true,
            elements: {
                bar: {
                    borderWidth: 2,
                },
            },
            plugins: {
                title: {
                    display: true,
                    text: stat.name
                },
                legend: {
                    display: false,
                }
            },
            scales: {
                yAxes: {
                    display: false,
                }
            },
        };

        let data = {
            labels: labels,
            datasets: [
                {
                    label: stat.name,
                    data: vals,
                    borderColor: '#00000000',
                    backgroundColor: colors,
                },
            ],
        };

        return (
            <div className='chart-container' style={{ height: height + "px"}}>
                <Bar options={options} data={data} />
            </div>
        );
    }
}