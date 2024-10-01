import React, { useState } from 'react';
import { AthletesApi, CreateAthleteRequest } from '../../apis';
import { Box, Button, Paper, TextField, Typography } from '@mui/material';
import { Stepper, Step, StepLabel } from '@mui/material';

interface Athlete {
    name: string;
    email: string;
    password: string;
    birth: Date;
    phone: string
}

const NewAthlete: React.FC = () => {
    const [athlete, setAthlete] = useState<Athlete>({ name: '', email: '', password: '', birth: new Date(), phone: '' });
    
    const steps = ['Dados do Atleta', 'Plano / Objetivo', 'Planilha'];

    const [activeStep, setActiveStep] = useState(0);
    
    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setAthlete({ ...athlete, [name]: value });
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        const newAthlete: CreateAthleteRequest = {
            name: athlete.name,
            birth: athlete.birth.toISOString(),
            phone: athlete.phone,
            email: athlete.email,
            password: athlete.password
        };
        try {
            const api = new AthletesApi();
            const response = await api.apiAthletesPost(newAthlete);
            console.log('Athlete created successfully:', response);
            // Optionally, redirect or show a success message
        } catch (error) {
            console.error('Error creating athlete:', error);
            // Optionally, show an error message
        }
    };

    const handleNext = () => {
        setActiveStep((prevActiveStep) => prevActiveStep + 1);
    };

    const handleBack = () => {
        setActiveStep((prevActiveStep) => prevActiveStep - 1);
    };

    const handleReset = () => {
        setActiveStep(0);
    };

    return (
        <Box sx={{ display: 'flex', justifyContent: 'center', width: '100%' }}>
            <Paper 
                sx={{ 
                    width: { xs: '100%', md: '50%' }, 
                    height: '100%', 
                    overflow: 'hidden', 
                    padding: '10px' 
                }}
            >
                <Box sx={{ display: 'flex', alignItems: 'center', marginBottom: '30px' }}>
                    {/* <PersonIcon fontSize="large" color='primary' /> Add the icon here */}
                    <Typography variant="h6">Novo Atleta</Typography>
                </Box>
                <Stepper sx={{ marginBottom: '15px' }} activeStep={activeStep}>
                    {steps.map((label, index) => (
                        <Step key={label}>
                            <StepLabel>{label}</StepLabel>
                        </Step>
                    ))}
                </Stepper>
                <form onSubmit={handleSubmit}>
                    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                        {activeStep === 0 && (
                            <>
                                <TextField
                                    label="Name"
                                    variant="outlined"
                                    id="name"
                                    name="name"
                                    value={athlete.name}
                                    onChange={handleChange}
                                    required
                                />
                                <TextField
                                    label="Email"
                                    variant="outlined"
                                    type="email"
                                    id="email"
                                    name="email"
                                    value={athlete.email}
                                    onChange={handleChange}
                                    required
                                />
                                <TextField
                                    label="Password"
                                    variant="outlined"
                                    type="password"
                                    id="password"
                                    name="password"
                                    value={athlete.password}
                                    onChange={handleChange}
                                    required
                                />
                                <TextField
                                    label="Birth Date"
                                    variant="outlined"
                                    type="date"
                                    id="birth"
                                    name="birth"
                                    value={athlete.birth.toISOString().split('T')[0]}
                                    onChange={handleChange}
                                    required
                                    InputLabelProps={{
                                        shrink: true,
                                    }}
                                />
                                <TextField
                                    label="Phone"
                                    variant="outlined"
                                    type="tel"
                                    id="phone"
                                    name="phone"
                                    value={athlete.phone}
                                    onChange={handleChange}
                                    required
                                />
                            </>
                        )}
                        {activeStep === 1 && (
                            <>
                                
                            </>
                        )}
                        {activeStep === 2 && (
                            <>
                                
                            </>
                        )}
                        <Box sx={{ display: 'flex', justifyContent: 'space-between', marginTop: 2 }}>
                            <Button
                                disabled={activeStep === 0}
                                onClick={handleBack}
                            >
                                Back
                            </Button>
                            {activeStep === steps.length - 1 ? (
                                <Button type="submit" variant="outlined" color="primary">
                                    Create Athlete
                                </Button>
                            ) : (
                                <Button variant="outlined" color="primary" onClick={handleNext}>
                                    Next
                                </Button>
                            )}
                        </Box>
                    </Box>
                </form>
            </Paper>
        </Box>
    );
};

export default NewAthlete;