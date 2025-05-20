import { CircularProgress } from '@mui/material';
import styled from '@emotion/styled';

const LoadContainer = styled.div`
    display: flex;
    flex: 1;
    justify-content: center;
    align-items: center;
`;

const Loading = () => {
    <LoadContainer>
        <CircularProgress color="primary" />
    </LoadContainer>
};

export default Loading;