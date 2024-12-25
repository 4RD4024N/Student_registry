import React from 'react';
import './index.css';
import ReactDOM from 'react-dom/client';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Login from './app/login/page'; // Default export edilen bileşen
import Register from './app/register/page'; // Default export edilen bileşen
import Students from './app/students/page';
import { Announcement } from './app/announcement/page';
import CourseList from './app/courselist/page';
import Chat from './app/chat/page';
import  { CourseSchedulePage } from './app/courseschedule/page';
import ResetPassword from './app/reset/page';
import ResetPasswordPage from './app/rst/page';
import NavigationPage from './app/navigationpage/page';


ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
  <React.StrictMode>
    <BrowserRouter>
      <Routes>
   
        <Route path="/" element={<Login />} />
        <Route path="/navigate" element={<NavigationPage />} />  
        <Route path="/register" element={<Register />} />
        <Route path="/studentinfo" element={<Students/>} /> 
        <Route path="/announcement" element={<Announcement/>} />
        <Route path="/courses" element={<CourseList />} />
        <Route path="/chat" element={<Chat/>} />
        <Route path="/courseschedule" element={<CourseSchedulePage/>}/>
        <Route path="/reset" element={<ResetPassword/>} />
        <Route path="/reset-password" element={<ResetPasswordPage/>} />

      </Routes>
    </BrowserRouter>
  </React.StrictMode>
);
