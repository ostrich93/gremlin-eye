import { Chart as ChartJS, CategoryScale, LinearScale, BarElement, Tooltip } from 'chart.js';
import { Bar } from 'react-chartjs-2';
import { Row } from 'react-bootstrap';

ChartJS.register(CategoryScale, LinearScale, BarElement, Tooltip);

const ReviewChart = ({ reviewScores }) => {

    const labels = ['0.5★', '1★', '1.5★', '2★', '2.5★', '3★', '3.5★', '4★', '4.5★', '5★'];

    const data = {
        labels: labels,
        datasets: [
            {
                data: reviewScores,
                backgroundColor: '#fc6399'
            }
        ]
    };

    const options = {
        responsive: true,
        scales: {
            x: {
                border: {
                    color: 'rgba(162,177,229,.2)'
                },
                grid: {
                    display: false
                },
                ticks: {
                    autoSkip: true
                }
            },
            y: {
                display: false,
                grid: {
                    display: true
                },
                min: 0,
                ticks: {
                    autoSkip: true
                }
            }
        }
    };

    return (
        <Row id="rating-bars-height" className="mx-0">
            <Bar data={data} options={options} />
        </Row>
    );
};

export default ReviewChart;